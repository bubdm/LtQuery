using DryIoc;

namespace LtQuery.ORM.DryIoc
{
    public class Module
    {
        public void Register(Container container)
        {
            var ltContext = new LtQueryContainer(container);
            container.RegisterInstance<ITableDefinitionRegistrator>(ltContext);
            container.RegisterInstance<ITableDefinitionResolver>(ltContext);
        }
    }
}
