using Core.Entities.Partners;
using Core.Enumerables;

namespace Core.Entities
{
    public class ClientCredential : Audit
    {
        /// <summary>
        /// Mã định danh duy nhất cho client (gửi lên khi request token)
        /// </summary>
        public string ClientId { get; set; } = null!;

        /// <summary>
        /// Mã bí mật đã mã hóa (hash). Không lưu plain text.
        /// </summary>
        public string ClientSecretHash { get; set; } = null!;

        /// <summary>
        /// Vai trò (phân quyền) của client, ví dụ: MobileShopManager, ZenPayCore, POS, Admin...
        /// </summary>
        public string Role { get; set; } = RoleEnum.Unknown;

        /// <summary>
        /// Trạng thái kích hoạt
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Mô tả client (để người quản trị dễ hiểu)
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Ràng buộc với Partner nếu là client do đối tác cấp
        /// </summary>
        public Guid? PartnerId { get; set; }

        public Partner? Partner { get; set; }
    }
}
