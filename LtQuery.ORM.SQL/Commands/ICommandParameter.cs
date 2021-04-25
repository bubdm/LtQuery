namespace LtQuery.ORM.SQL.Commands
{
    interface ICommandParameter<TDynamic>
    {
        void SetParameter(TDynamic values);
    }
}
