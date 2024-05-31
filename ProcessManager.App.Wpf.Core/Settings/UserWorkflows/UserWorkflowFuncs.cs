using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows
{
    public static class UserWorkflowFuncs
    {
        public static Result<bool> UserHasAccessToCreatingNewWorkflow(User currentUser, Workflow workflow)
        {
            var currentUserGroupsIds = currentUser.UserGroups.Select(x => x.Id)
                .ToList();
            var canCreateWorkflowUserGroups = workflow.CanCreateUserGroups.Select(x => x.Id)
                .ToList();
            if (!canCreateWorkflowUserGroups.Any(x => currentUserGroupsIds.Contains(x)))
                return Result<bool>.Failure(false, "Brak uprawnień do rozpoczęcia przepływu");

            return Result<bool>.Success(true);
        }

        public static bool CanProcessStage(
        User currentUser,
        WorkflowStage currentStage,
        UserWorkflowData userWorkflowData
        )
        {
            if (userWorkflowData.WorkflowStatus is UserWorkflowDataStatus.Complete)
                return false;
            return IsAssignedToWorkflow(currentUser, currentStage, userWorkflowData);
        }

        private static bool IsAssignedToWorkflow(User currentUser, WorkflowStage currentStage, UserWorkflowData userWorkflowData)
        {
            switch (currentStage.AssigneeType)
            {
                case WorkflowStageAssignee.Creator:
                    return userWorkflowData.UserId == currentUser.Id;
                case WorkflowStageAssignee.SpecificUser:
                    return currentStage.AssigneeId == currentUser.Id;
                case WorkflowStageAssignee.Group:
                    return currentUser.UserGroups.Any(x => x.Id == currentStage.AssigneeId);
                default:
                    return false;
            }
        }

        public static bool CanEditField(
        WorkflowStage currentStage,
        UserWorkflowStageFieldValueData fieldToSave
        )
        {
            var fieldConfiguration = currentStage.Configurations.First(x => x.WorkflowStageId == currentStage.Id && x.FieldCode == fieldToSave.FieldCode);

            return fieldConfiguration.IsEditable;
        }

        public static bool CanSeeField(
        WorkflowStage currentStage,
        UserWorkflowStageFieldValueData fieldToSave
        )
        {
            var fieldConfiguration = currentStage.Configurations.First(x => x.WorkflowStageId == currentStage.Id && x.FieldCode == fieldToSave.FieldCode);

            return fieldConfiguration.IsVisible;
        }

        public static bool CanForwardStage(Workflow workflow, WorkflowStage currentStage)
        {
            var lastStage = workflow.Stages.OrderByDescending(x => x.Index).First();

            return currentStage.Id != lastStage.Id;
        }  

        public static bool CanCompleteWorkflow(Workflow workflow, WorkflowStage currentStage)
        {
            var lastStage = workflow.Stages.OrderByDescending(x => x.Index).First();

            return currentStage.Id == lastStage.Id;
        }

        public static bool CanRevertStage(Workflow workflow, WorkflowStage currentStage)
        {
            var lastStage = workflow.Stages.OrderBy(x => x.Index).First();

            return currentStage.Id != lastStage.Id;
        }
    }

    
}
