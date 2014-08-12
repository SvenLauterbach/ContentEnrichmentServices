using Microsoft.Office.Server.Search.ContentProcessingEnrichment;
using Microsoft.Office.Server.Search.ContentProcessingEnrichment.PropertyTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace ImageCEWS
{
    /// <summary>
    /// CEWS for extracting metadata from images.
    /// </summary>
    public class ImageCEWS : IContentProcessingEnrichmentService
    {
        private readonly ProcessedItem processedItemHolder = new ProcessedItem
        {
            ItemProperties = new List<AbstractProperty>()
        };

        public ProcessedItem ProcessItem(Item item)
        {
            if (item.RawData != null)
            {
                MemoryStream rawData = new MemoryStream(item.RawData);
                BitmapMetadata metadata = GetImageMetadata(rawData);

                if (!string.IsNullOrEmpty(metadata.Subject))
                {
                    Property<string> subject = new Property<string>();
                    subject.Value = metadata.Subject;
                    subject.Name = "subject";
                    processedItemHolder.ItemProperties.Add(subject);
                }

                if (!string.IsNullOrEmpty(metadata.Comment))
                {
                    Property<string> comment = new Property<string>();
                    comment.Value = metadata.Comment;
                    comment.Name = "comment";
                    processedItemHolder.ItemProperties.Add(comment);
                }
            }

            return processedItemHolder;
        }

        /// <summary>
        /// Returns metadata from an stream containing the raw image.
        /// </summary>
        /// <param name="image">Stream which contains the raw data of the image</param>
        /// <returns>Metadata of image.</returns>
        private BitmapMetadata GetImageMetadata(Stream image)
        {
            BitmapSource img = BitmapFrame.Create(image);
            BitmapMetadata metadata = (BitmapMetadata)img.Metadata;
            return metadata;
        }
    }
}
