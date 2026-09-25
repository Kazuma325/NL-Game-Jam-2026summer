using System;
using System.Collections.Generic;
using ShopGame.Manual.Data;
using UnityEngine;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualPageRepository
    {
        private readonly Dictionary<string, ManualPageData> pagesById;

        public ManualPageRepository(
            ManualParser parser,
            IReadOnlyCollection<TextAsset> sources)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            pagesById =
                new Dictionary<string, ManualPageData>();

            foreach (TextAsset source in sources)
            {
                if (source == null)
                {
                    throw new ArgumentException(
                        "Manual source collection must not contain null entries.",
                        nameof(sources));
                }

                ManualPageData page =
                    parser.Parse(source.text);

                if (pagesById.ContainsKey(page.Id))
                {
                    throw new ArgumentException(
                        $"Duplicate manual page ID: {page.Id}",
                        nameof(sources));
                }

                pagesById.Add(
                    page.Id,
                    page);
            }
        }

        public bool HasPage(string pageId)
        {
            ValidatePageId(pageId);

            return pagesById.ContainsKey(pageId);
        }

        public ManualPageData GetPage(string pageId)
        {
            ValidatePageId(pageId);

            if (!pagesById.TryGetValue(
                pageId,
                out ManualPageData page))
            {
                throw new InvalidOperationException(
                    $"Manual page was not found: {pageId}");
            }

            return page;
        }

        public IReadOnlyCollection<ManualPageData> GetAllPages()
        {
            return pagesById.Values;
        }

        private static void ValidatePageId(
            string pageId)
        {
            if (string.IsNullOrWhiteSpace(pageId))
            {
                throw new ArgumentException(
                    "Page ID must not be null, empty, or whitespace.",
                    nameof(pageId));
            }
        }
    }
}