﻿using System.Collections.Generic;

namespace FastDFSCore.Client
{
    /// <summary>
    ///查询更新文件时的Storage
    ///Reqeust 
    ///    Cmd: TRACKER_PROTO_CMD_SERVICE_QUERY_UPDATE 103
    ///    Body:
    ///    @ FDFS_GROUP_NAME_MAX_LEN bytes:  group name
    ///    @ filename bytes: filename
    ///Response
    ///    Cmd: TRACKER_PROTO_CMD_RESP
    ///    Status: 0 right other wrong
    ///    Body: 
    ///    @ FDFS_GROUP_NAME_MAX_LEN bytes: group name
    ///    @ IP_ADDRESS_SIZE - 1 bytes: storage server ip address
    ///    @ FDFS_PROTO_PKG_LEN_SIZE bytes: storage server port
    /// </summary>
    public class QueryUpdateRequest : FDFSRequest<QueryUpdateResponse>
    {
        /// <summary>组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>文件FileId
        /// </summary>
        public string FileId { get; set; }

        /// <summary>Ctor
        /// </summary>
        public QueryUpdateRequest()
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="fileId">文件FileId</param>
        public QueryUpdateRequest(string groupName, string fileId)
        {
            GroupName = groupName;
            FileId = fileId;
        }

        /// <summary>EncodeBody
        /// </summary>
        public override byte[] EncodeBody(FDFSOption option)
        {
            byte[] groupNameBuffer = Util.CreateGroupNameBuffer(option.Charset, GroupName);
            byte[] fileIdBuffer = Util.StringToByte(option.Charset, FileId);
            var length = Consts.FDFS_GROUP_NAME_MAX_LEN + fileIdBuffer.Length;
            List<byte> bodyBuffer = new List<byte>();
            bodyBuffer.AddRange(groupNameBuffer);
            bodyBuffer.AddRange(fileIdBuffer);

            Header = new FDFSHeader(length, Consts.TRACKER_PROTO_CMD_SERVICE_QUERY_UPDATE, 0);
            return bodyBuffer.ToArray();
        }
    }
}
