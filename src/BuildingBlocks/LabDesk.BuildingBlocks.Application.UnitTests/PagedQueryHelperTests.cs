using LabDesk.BuildingBlocks.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
namespace LabDesk.BuildingBlocks.Application.UnitTests
{
    public class PagedQueryHelperTests
    {
        [Theory]
        [InlineData(1, 5, 0, 5)]
        [InlineData(3, 10, 20, 10)]
        [InlineData(null, 20, 0, 20)]
        [InlineData(5, null, 0, int.MaxValue)]
        [InlineData(null, null, 0, int.MaxValue)]
        public void PagedQueryHelper_GetPageData_Test(int? page, int? perPage, int offset, int next)
        {
            // Given
            IPagedQuery query = new TestQuery(page, perPage);

            // When
            var pageData = PagedQueryHelper.GetPageData(query);

            // Then
            pageData.Should().BeEquivalentTo(new PageData(offset, next));
        }

        private class TestQuery : IPagedQuery
        {
            public TestQuery(int? page, int? perPage)
            {
                Page = page;
                PerPage = perPage;
            }

            public int? Page { get; }
            public int? PerPage { get; }
        }
    }
}
