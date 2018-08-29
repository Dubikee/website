using System.Collections.Generic;

namespace Server.Shared.Core.Database
{
    public interface IWhutDbContext<T> where T : IStudent
    {
        /// <summary>
        /// 所有学生
        /// </summary>
        IEnumerable<T> Students { get; }

        /// <summary>
        /// 通过账号查找学生
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        T FindStudent(string uid);

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        bool AddStudent(T student);

        /// <summary>
        /// 删除学生
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool DeleteStudent(string uid);

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        bool UpdateStudent(T student);
    }
}
