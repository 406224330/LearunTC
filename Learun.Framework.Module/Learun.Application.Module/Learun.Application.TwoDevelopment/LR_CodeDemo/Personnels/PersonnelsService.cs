﻿using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-28 21:48
    /// 描 述：个人基本信息
    /// </summary>
    public class PersonnelsService : RepositoryFactory
    {
        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// </summary>
        /// <param name="pagination">查询参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<tc_PersonnelsEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.F_PersonId,
                t.F_UserName,
                t.F_IDCardNo,

 
        FLOOR(DATEDIFF(DY, substring(t.F_IDCardNo,7,4), GETDATE()) / 365.25) AS F_Age, 
        case
 when len(t.F_IDCardNo) = 18 and cast(substring(t.F_IDCardNo,17,1) as int) % 2 = 0 then '1'
when len(t.F_IDCardNo) = 18 and cast(substring(t.F_IDCardNo,17,1) as int) % 2 = 1 then '2'
else null end  AS F_Gender,


        
                t.F_PlaceCode,
                t.F_CertCode,
                t.F_ApplicantId,
                t.F_SceneType,
                t.F_Description
                ");
                strSql.Append("  FROM tc_Personnels t ");
                strSql.Append("  WHERE 1=1 ");
                strSql.Append("  AND t.F_DeleteMark=0 ");
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                var dp = new DynamicParameters(new { });
                if (!queryParam["F_UserName"].IsEmpty())
                {
                    dp.Add("F_UserName", "%" + queryParam["F_UserName"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_UserName Like @F_UserName ");
                }
                if (!queryParam["F_IDCardNo"].IsEmpty())
                {
                    dp.Add("F_IDCardNo", "%" + queryParam["F_IDCardNo"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_IDCardNo Like @F_IDCardNo ");
                }
                if (!queryParam["F_PersonId"].IsEmpty())
                {
                    dp.Add("F_PersonId", queryParam["F_PersonId"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_PersonId = @F_PersonId ");
                }
                if (!queryParam["F_PlaceCode"].IsEmpty())
                {
                    dp.Add("F_PlaceCode", "%" + queryParam["F_PlaceCode"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_PlaceCode Like @F_PlaceCode ");
                }
                if (!queryParam["F_SceneType"].IsEmpty())
                {
                    dp.Add("F_SceneType",queryParam["F_SceneType"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_SceneType = @F_SceneType ");
                }
                if (!queryParam["F_CertCode"].IsEmpty())
                {
                    dp.Add("F_CertCode", "%" + queryParam["F_CertCode"].ToString() + "%", DbType.String);
                    strSql.Append(" AND t.F_CertCode Like @F_CertCode ");
                }
                if (!queryParam["F_ApplicantId"].IsEmpty())
                {
                    dp.Add("F_ApplicantId",queryParam["F_ApplicantId"].ToString(), DbType.String);
                    strSql.Append(" AND t.F_ApplicantId = @F_ApplicantId ");
                }
                return this.BaseRepository().FindList<tc_PersonnelsEntity>(strSql.ToString(),dp, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }


        public IEnumerable<tc_PersonnelsEntity> GetPageList(string ApplicantId)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(@"
                t.F_PersonId,
                t.F_UserName,
                t.F_IDCardNo,
 
        DATEDIFF(year, CONVERT(smalldatetime, SUBSTRING(ISNULL(t.F_IDCardNo, 0), 7, 8)), GETDATE()) AS F_Age, 
        CASE LEFT(RIGHT(t.F_IDCardNo, 2), 1) % 2 WHEN 1 THEN '2' ELSE '1' END AS F_Gender,

        
                t.F_PlaceCode,
                t.F_CertCode,
                t.F_ApplicantId,
                t.F_SceneType,
                t.F_Description
                ");
                strSql.Append("  FROM tc_Personnels t ");
                strSql.Append("  WHERE 1=1 ");
                strSql.Append("  AND t.F_DeleteMark=0 ");
                if (!string.IsNullOrEmpty(ApplicantId))
                {
                    strSql.AppendFormat(" AND t.F_ApplicantId = '{0}' ",ApplicantId);
                }
                return this.BaseRepository().FindList<tc_PersonnelsEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取tc_Personnels表实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public tc_PersonnelsEntity Gettc_PersonnelsEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity<tc_PersonnelsEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSqlTree()
        {
            try
            {
                return this.BaseRepository().FindTable(" select * from dbo.tc_Applicant where F_ApplicantType=1 ");
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                tc_PersonnelsEntity entity = new tc_PersonnelsEntity()
                {
                    F_PersonId = keyValue,
                    F_DeleteMark = 1
                };
                this.BaseRepository().Update(entity);
               
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void SaveEntity(string keyValue, tc_PersonnelsEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

    }
}
