using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Reporting
{
    public class Table
    {
    }
    public interface Criterion
    {
    }
    public class AndCriterion: Criterion
    {
        public AndCriterion(Criterion lhs, Criterion rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
        public Criterion Lhs { get; set; }
        public Criterion Rhs { get; set; }
        public override string ToString()
        {
            return $" {Lhs} and {Rhs}";
        }
    }
    public class EqualCriterion: Criterion
    {
        public EqualCriterion(string col, string value)
        {
            Col = col;
            Value = value;
        }
        public string Col { get; set; }
        public string Value { get; set; }
    }
    public class LikeCriterion: Criterion
    {
        public LikeCriterion(string col, string value)
        {
            Col = col;
            Value = value;
        }
        public string Col { get; set; }
        public string Value { get; set; }
    }
    public class LibraryQuery
    {
        private string NonBinaryCriterionString(Criterion criterion)
        {
            if (criterion.GetType() == typeof(EqualCriterion))
                return $"{((EqualCriterion)criterion).Col} = '{((EqualCriterion)criterion).Value}'";
            if (criterion.GetType() == typeof(LikeCriterion))
                return $"{((LikeCriterion)criterion).Col} like '{((LikeCriterion)criterion).Value}%'";
            return criterion.ToString();
        }

        public IList<Material> Retrieve(string[] cols, string tbl, Criterion criterion)
        {
            var sql = "select ";
            var cs = "";
            var i = 0;
            while (i < cols.Length)
            {
                if (cs.Any())
                {
                    cs = "" + cs + ", ";
                }
                var c = cols[i++];
                cs = cs + c.ToString();
            }
            sql += cs;
            sql += " " + "from";
            sql = sql + " " + tbl;
            if (criterion != null)
                sql += " where ";
            sql += " ";
            Criterion lhs, rhs;
            while (criterion != null)
            {
                if (criterion.GetType() == typeof(AndCriterion))
                {
                    lhs = ((AndCriterion)criterion).Lhs; 
                    rhs = ((AndCriterion)criterion).Rhs; 
                    sql = sql + $" { NonBinaryCriterionString(lhs).ToString() } " 
                        + $"and { NonBinaryCriterionString(rhs).ToString() }";
                } else
                {
                    sql+= NonBinaryCriterionString(criterion);
                }
                criterion = null;
            }
            //sql = sql + criterion.ToString();
            return Database.SelectMany(sql);
        }
    }
}