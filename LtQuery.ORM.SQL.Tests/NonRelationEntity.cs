namespace LtQuery.ORM.SQL.Tests
{
    class NonRelationEntity
    {
        public long Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public bool Equals(NonRelationEntity other)
        {
            if (Id != other.Id)
                return false;
            if (Code != other.Code)
                return false;
            if (Name != other.Name)
                return false;
            return true;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is NonRelationEntity))
                return false;
            return Equals((NonRelationEntity)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var code = Id.GetHashCode();
                code = (code * 5) ^ Code;
                code = (code * 5) ^ Name?.GetHashCode() ?? 0;
                return code;
            }
        }
    }
}
