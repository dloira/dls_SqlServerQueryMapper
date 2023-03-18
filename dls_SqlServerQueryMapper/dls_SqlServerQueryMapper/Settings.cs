using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlQueryMapper
{
    /// <summary>
    /// Settings
    /// </summary>
    internal class Settings
    {
        // Configuration keys
        private const string QUERY_XML_FILE_PATH_KEY = "CommonDataAccess:QueryXmlFilePath";
        private const string BLOCK_SIZE_UPDATE_KEY = "CommonDataAccess:BlockSizeUpdate";
        private const string MAX_TEMPORARY_TABLE_ROWS_KEY = "CommonDataAccess:MaxTemporaryTableRows";

        /// <summary>
        /// Instantiate a new settings
        /// </summary>
        /// <param name="configuration">configuration to use defined variables</param>
        public Settings(IConfiguration configuration = null)
        {
            if (configuration == null)
            {   
                QueryXmlFilePath = "QueryXmlFilePath-Not-Specified_Use-Configuration-To-Instantiate-Settings-Object";
                BlockSizeUpdate = 500;
                MaxTemporaryTableRows = 100000;
            }
            else
            {   // set the path of XML file with queries
                QueryXmlFilePath = configuration[QUERY_XML_FILE_PATH_KEY];

                // set the number of rows processed on block updates
                if (!int.TryParse(configuration[BLOCK_SIZE_UPDATE_KEY], out int blockSizeUpdate))
                {
                    blockSizeUpdate = 500;
                }
                BlockSizeUpdate = blockSizeUpdate;

                // set the maximun rows to insert on data acces methods 
                if (!int.TryParse(configuration[MAX_TEMPORARY_TABLE_ROWS_KEY], out int maxTemporaryTableRows))
                {
                    maxTemporaryTableRows = 100000;
                }
                MaxTemporaryTableRows = maxTemporaryTableRows;
            }

        }

        /// <summary>
        /// Path of XML file with SQL queries
        /// </summary>
        public string QueryXmlFilePath { get; }

        /// <summary>
        /// Number of rows to process on block updates
        /// </summary>
        public int BlockSizeUpdate { get; }

        /// <summary>
        /// Maximun number of rows for temporary tables created by DataAccess methods
        /// </summary>
        public int MaxTemporaryTableRows { get; }

    }
}
