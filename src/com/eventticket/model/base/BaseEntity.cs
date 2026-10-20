using System;

namespace EventTicket.Model.Base
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string TrangThai { get; set; }

        protected BaseEntity()
        {
            NgayCapNhat = DateTime.Now;
            NgayTao = DateTime.Now;
            TrangThai = "ACTIVE";
        }

        public abstract bool Validate();

        // equals
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            BaseEntity that = (BaseEntity)obj;
            return Id != null && Id.Equals(that.Id);
        }

        // hashCode
        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }
    }
}
