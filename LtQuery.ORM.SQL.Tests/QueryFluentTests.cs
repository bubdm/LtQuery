using System;
using Xunit;

namespace LtQuery.ORM.SQL.Tests
{
    using QueryElements;

    public class QueryFluentTests
    {
        [Fact]
        public void WhereWithEqual()
        {
            var expected = new Query<NonRelationEntity>(where: new EqualOperator(new Property<NonRelationEntity>(nameof(NonRelationEntity.Id)), new Parameter("Id")));

            var actual = Lt.Query<NonRelationEntity>().Where(_ => _.Id == Lt.Arg<int>()).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhereWithEqualWithParameterName()
        {
            var expected = new Query<NonRelationEntity>(where: new EqualOperator(new Property<NonRelationEntity>(nameof(NonRelationEntity.Id)), new Parameter("Id")));

            var actual = Lt.Query<NonRelationEntity>().Where(_ => _.Id == Lt.Arg<int>("Id")).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrderBy()
        {
            var expected = new Query<NonRelationEntity>(orderBy: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Id")));

            var actual = Lt.Query<NonRelationEntity>().OrderBy(_ => _.Id).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrderByDescending()
        {
            var expected = new Query<NonRelationEntity>(orderBy: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Id"), OrderDirect.Desc));

            var actual = Lt.Query<NonRelationEntity>().OrderByDescending(_ => _.Id).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenBy()
        {
            var expected = new Query<NonRelationEntity>(orderBy: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Id"), then: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Code"))));

            var actual = Lt.Query<NonRelationEntity>().OrderBy(_ => _.Id).ThenBy(_ => _.Code).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenByWithoutOrderBy()
        {
            try
            {
                var actual = Lt.Query<NonRelationEntity>().OrderBy(_ => _.Id).ThenBy(_ => _.Code).ToImmutable();
                throw new Exception();
            }
            catch { }
        }

        [Fact]
        public void ThenByDescending()
        {
            var expected = new Query<NonRelationEntity>(orderBy: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Id"), then: new OrderBy<NonRelationEntity>(new Property<NonRelationEntity>("Code"), OrderDirect.Desc)));

            var actual = Lt.Query<NonRelationEntity>().OrderBy(_ => _.Id).ThenByDescending(_ => _.Code).ToImmutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenByDescendingWithoutOrderBy()
        {
            try
            {
                var actual = Lt.Query<NonRelationEntity>().OrderBy(_ => _.Id).ThenByDescending(_ => _.Code).ToImmutable();
                throw new Exception();
            }
            catch { }
        }
    }
}
