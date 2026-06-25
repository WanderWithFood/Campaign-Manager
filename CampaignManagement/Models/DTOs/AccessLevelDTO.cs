namespace CampaignManagement.Models.DTOs
{
    public class MainMenuDTO
    {
        public int? mstMainMenuId { get; set; }
        public string menuName { get; set; } = null!;
        public string? menuIcon { get; set; }
        public int displayOrder { get; set; }
        public bool isActive { get; set; } = true;
        public int createdBy { get; set; }
    }

    public class SubMenuDTO
    {
        public int? mstSubMenuId { get; set; }
        public int mstMainMenuId { get; set; }
        public string subMenuName { get; set; } = null!;
        public string pageUrl { get; set; } = null!;
        public int displayOrder { get; set; }
        public bool isActive { get; set; } = true;
        public int createdBy { get; set; }
    }

    public class AccessPermissionDTO
    {
        public int? mstAccessLevelPagePermissionId { get; set; }
        public int accessLevelId { get; set; }
        public int mstMainMenuId { get; set; }
        public int mstSubMenuId { get; set; }
        public bool canView { get; set; }
        public bool canViewOthers { get; set; }
        public bool canCreate { get; set; }
        public bool canEdit { get; set; }
        public bool canEditOthers { get; set; }
        public bool canDelete { get; set; }
        public bool canDeleteOthers { get; set; }
        public bool canViewAllRecords { get; set; }
        public int createdBy { get; set; }
    }

    public class MenuSubMenuForAccessPrivilegeDTO
    {
        public int mstMainMenuId { get; set; }
        public string mainMenuName { get; set; } = null!;
        public int mstSubMenuId { get; set; }
        public string subMenuName { get; set; } = string.Empty;
    }

    public class MenuItemWithPermissionDTO
    {
        public int mstMainMenuId { get; set; }
        public string menuName { get; set; } = null!;
        public string? menuIcon { get; set; }
        public int displayOrder { get; set; }
        public List<SubMenuItemWithPermissionDTO> SubMenus { get; set; } = new();
    }

    public class SubMenuItemWithPermissionDTO
    {
        public int mstSubMenuId { get; set; }
        public string subMenuName { get; set; } = null!;
        public string pageUrl { get; set; } = null!;
        public int displayOrder { get; set; }
        public bool canView { get; set; }
    }

    public class PagePermissionsDTO
    {
        public bool CanView { get; set; }
        public bool CanViewOthers { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanEditOthers { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDeleteOthers { get; set; }
        public bool CanViewAllRecords { get; set; }
        public List<int> AllowedUserIds { get; set; } = new();
    }
}
