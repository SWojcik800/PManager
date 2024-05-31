using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows
{
    public sealed class UserWorkflowAggregate
    {
        List<UserWorkflowData> _activeUserWorkflows;
        Workflow _workflow;
        User _user;
        UserWorkflowData _userWorkflowData;
        private int _lastUserWorkflowId;
        private Func<DateTime> _getNowFunc;
        public static Result<UserWorkflowAggregate> CreateForUser(
            List<UserWorkflowData> activeUserWorkflows, 
            Workflow workflow,
            User currentUser,
            Func<DateTime> getNowFunc,
            int lastUserWorkflowId
            )
        {
            var instance = new UserWorkflowAggregate();
            instance._getNowFunc = getNowFunc;
            instance._lastUserWorkflowId = lastUserWorkflowId;
            instance._activeUserWorkflows = activeUserWorkflows;
            instance._workflow = workflow;
            instance._user = currentUser; 
            instance._userWorkflowData = new UserWorkflowData();
            instance._userWorkflowData.User = currentUser;
            instance._userWorkflowData.CreationTime = getNowFunc();
            instance._userWorkflowData.WorkflowId = workflow.Id;

            if(instance._activeUserWorkflows.Any(x => x.WorkflowId == instance._workflow.Id && x.Status == Shared.Enums.EntityStatus.Active))
            {
                return Result<UserWorkflowAggregate>.Failure(null, "Istnieje już aktywny przepływ tego typu dla tego użytkownika");
            }

            var hasAccessToCreateNewWorkflowResult = UserWorkflowFuncs.UserHasAccessToCreatingNewWorkflow(instance._user, instance._workflow);

            if (!hasAccessToCreateNewWorkflowResult.IsSuccess)
                return Result<UserWorkflowAggregate>.Failure(null, hasAccessToCreateNewWorkflowResult.ErrorMessage);

            var firstStage = instance._workflow.Stages.OrderBy(x => x.Index).First();
            instance._userWorkflowData.CurrentStage = firstStage;
            instance._userWorkflowData.CurrentStageId = firstStage.Id;
            instance.CalculateWorkflowCode();

            return Result<UserWorkflowAggregate>.Success(instance);
        }

        private void CalculateWorkflowCode()
        {
            var mask = _workflow.CodeMask;

            var id = _lastUserWorkflowId == 0 ? 1 : _lastUserWorkflowId + 1;

            var code = mask.Replace("{WorkflowTypeId}", _lastUserWorkflowId.ToString())
                .Replace("{DD}", _userWorkflowData.CreationTime.ToString("dd"))
                .Replace("{MM}", _userWorkflowData.CreationTime.ToString("MM"))
                .Replace("{YYYY}", _userWorkflowData.CreationTime.ToString("yyyy"));

            _userWorkflowData.Code = code;
        }

        public UserWorkflowData GetDataToSave()
        {
            return _userWorkflowData;
        }

        public static Result<UserWorkflowAggregate> ReCreateFromData(List<UserWorkflowData> activeUserWorkflows, Workflow workflow, User user, UserWorkflowData workflowData, Func<DateTime> getNowFunc)
        {
            var instance = new UserWorkflowAggregate();
            instance._getNowFunc = getNowFunc;
            instance._activeUserWorkflows = activeUserWorkflows;
            instance._workflow = workflow;
            instance._user = user;
            instance._userWorkflowData = workflowData;

            return Result<UserWorkflowAggregate>.Success(instance);
        }

        public Result<UserWorkflowData> ForwardToNextStage()
        {
            var currentStage = _workflow.Stages.First(x => x.Id == _userWorkflowData.CurrentStageId);
            var canUserForwardStage = UserWorkflowFuncs.CanProcessStage(_user, currentStage, _userWorkflowData);

            if (!canUserForwardStage)
                return Result<UserWorkflowData>.Failure(null, "Brak uprawnień");


            if (currentStage.Index == _workflow.Stages.Select(x => x.Id).Max())
                return Result<UserWorkflowData>.Failure(null, "To jest ostatni etap");


            var nextStage = _workflow.Stages.Where(x => x.Index == currentStage.Index + 1).First();

            _userWorkflowData.CurrentStageId = nextStage.Id;

            return Result<UserWorkflowData>.Success(_userWorkflowData);
        }

        public Result<UserWorkflowData> RejectToPreviousStage()
        {
            var currentStage = _workflow.Stages.First(x => x.Id == _userWorkflowData.CurrentStageId);
            var canUserRejectStage = UserWorkflowFuncs.CanProcessStage(_user, currentStage, _userWorkflowData);

            if (!canUserRejectStage)
                return Result<UserWorkflowData>.Failure(null, "Brak uprawnień");


            if (currentStage.Index == _workflow.Stages.Select(x => x.Id).Min())
                return Result<UserWorkflowData>.Failure(null, "Nie można cofnąć ostatniego etapu");


            var previousStage = _workflow.Stages.Where(x => x.Index == currentStage.Index - 1).First();

            _userWorkflowData.CurrentStageId = previousStage.Id;

            return Result<UserWorkflowData>.Success(_userWorkflowData);
        }

        public Result<UserWorkflowData> CompleteWorkflow()
        {
            var lastStage = _workflow.Stages.OrderByDescending(x => x.Index).First();
            if (_userWorkflowData.CurrentStage.Id != lastStage.Id)
                return Result<UserWorkflowData>.Failure(default, "Nie można zakończyć przepływu ponieważ nie zakończono wszystkich etapów");

            _userWorkflowData.Status = Shared.Enums.EntityStatus.NotActive;
            _userWorkflowData.WorkflowStatus = UserWorkflowDataStatus.Complete;
            _userWorkflowData.CompletionDate = _getNowFunc();
            _userWorkflowData.CompletedByUserId = _user.Id;

            return Result<UserWorkflowData>.Success(_userWorkflowData);
        }

        public Result<UserWorkflowData> SaveFieldValues(List<UserWorkflowStageFieldValueData> data)
        {
            var currentStage = _workflow.Stages.First(x => x.Id == _userWorkflowData.CurrentStageId);

            foreach (var field in data)
            {
                if(UserWorkflowFuncs.CanEditField(currentStage, field))
                {
                    var existingFieldValue = _userWorkflowData.FieldValues.FirstOrDefault(x => x.FieldCode == field.FieldCode);
                    if(existingFieldValue is not null)
                    {
                        existingFieldValue.FieldValue = field.FieldValue;
                    } else
                    {
                        var fieldData = new UserWorkflowStageFieldValueData()
                        {
                            FieldCode = field.FieldCode,
                            FieldValue = field.FieldValue
                        };

                        _userWorkflowData.FieldValues.Add(fieldData);
                    }
                }
            }

            return Result<UserWorkflowData>.Success(_userWorkflowData);
        }



    }
}
