namespace userInterface
{
    class PartManager : Singleton<PartManager>
    {
        PartGroup selectedPartGroup = PartGroup.None;
        PartGroupManager partGroupController;
        WorkspaceEditor workspaceEditor;

        new void Awake()
        {
            workspaceEditor = WorkspaceEditor.GetInstance();
            partGroupController = PartGroupManager.GetInstance();
            partGroupController.gameObject.SetActive(false);
        }

        public void SelectPartGroup(PartGroup partGroup)
        {
            if (selectedPartGroup == partGroup)
            {
                selectedPartGroup = PartGroup.None;
                workspaceEditor.SetDesiredAction(WorkspaceAction.None);
            }
            else
            {
                selectedPartGroup = partGroup;
            }
            partGroupController.DrawPartGroup(selectedPartGroup);
        }
    }
}
