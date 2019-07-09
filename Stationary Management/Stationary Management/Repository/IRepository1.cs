using System;

namespace SCHM.Repo
{
    public interface IRepository1
    {
        int GetCount(DateTime date);
        int GetCount(int year);
    }
}