namespace Shared.Authorization
{
    using System.ComponentModel.DataAnnotations;

    namespace Shared.Authorization
    {
        public enum RoleCode
        {
            [Display(Name = "Chủ cửa hàng")]
            Owner,

            [Display(Name = "Quản lý cửa hàng")]
            Manager,

            [Display(Name = "Nhân viên bán hàng")]
            Cashier,

            [Display(Name = "Nhân viên kho")]
            InventoryStaff,

            [Display(Name = "Kế toán")]
            Accountant,

            [Display(Name = "CSKH")]
            CustomerSupport,

            [Display(Name = "Quản trị hệ thống Zenshop")]
            Admin
        }
    }

}
