using System;
using System.Collections.Generic;
using ShopGame.Manual.Data;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualSearchManager
    {
        private readonly ManualPageRepository pageRepository;

        public ManualSearchManager(
            ManualPageRepository pageRepository)
        {
            this.pageRepository =
                pageRepository
                ?? throw new ArgumentNullException(
                    nameof(pageRepository));
        }

        public IReadOnlyList<ManualPageData> Search(
            string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Array.Empty<ManualPageData>();
            }

            string searchKeyword =
                keyword.Trim();

            var results =
                new List<ManualPageData>();

            foreach (ManualPageData page in
                     pageRepository.GetAllPages())
            {
                foreach (string pageKeyword in page.Keywords)
                {
                    if (string.Equals(
                        pageKeyword,
                        searchKeyword,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(page);
                        break;
                    }
                }
            }

            return results;
        }
    }
}