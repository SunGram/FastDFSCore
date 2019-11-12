using System;

namespace FastDFSCore.Client
{
    /// <summary>附加文件返回
    /// </summary>
    public class AppendFileResponse : FDFSResponse
    {
        /// <summary>组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>文件FileId
        /// </summary>
        public string FileId { get; set; }

        /// <summary>Ctor
        /// </summary>
        public AppendFileResponse()
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="fileId">文件FileId</param>
        public AppendFileResponse(string groupName, string fileId)
        {
            GroupName = groupName;
            FileId = fileId;
        }

        /// <summary>LoadContent
        /// </summary>
        public override void LoadContent(FDFSOption option, byte[] data)
        {

        }
    }
}
