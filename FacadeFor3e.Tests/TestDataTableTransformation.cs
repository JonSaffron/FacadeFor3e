using System;
using System.Xml;
using NUnit.Framework;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestDataTableTransformation
        {
        [Test]
        public void TestNoData()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data />");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(0, dt.Columns.Count);
            Assert.AreEqual(0, dt.Rows.Count);
            }

        [Test]
        public void TestSingleRowAndColumn()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(1, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0][0]);
            }

        [Test]
        public void TestTwoColumnsAndOneRow()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number><DisplayName>my matter</DisplayName></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(2, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);
            Assert.AreEqual("DisplayName", dt.Columns[1].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0][0]);
            Assert.AreEqual("my matter", dt.Rows[0][1]);
            }

        [Test]
        public void TestTwoRowsAndOneColumns()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number></Matter><Matter><Number>5678</Number></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(1, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0][0]);
            Assert.AreEqual("5678", dt.Rows[1][0]);
            }

        [Test]
        public void TestTwoRowsAndTwoColumns()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number><DisplayName>my first matter</DisplayName></Matter><Matter><Number>5678</Number><DisplayName>my second matter</DisplayName></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(2, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);
            Assert.AreEqual("DisplayName", dt.Columns[1].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0][0]);
            Assert.AreEqual("my first matter", dt.Rows[0][1]);
            Assert.AreEqual("5678", dt.Rows[1][0]);
            Assert.AreEqual("my second matter", dt.Rows[1][1]);
            }

        [Test]
        public void TestDuplicateColumnName()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number><Number>1234</Number></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(2, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);
            Assert.AreEqual("Number1", dt.Columns[1].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0][0]);
            Assert.AreEqual("1234", dt.Rows[0][1]);
            }

        [Test]
        public void TestLongDataValue()
            {
            var xmlDoc = new XmlDocument();
            var longData = new string('.', 1024 * 20);
            xmlDoc.LoadXml($"<Data><Matter><Narrative>{longData}</Narrative></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(1, dt.Columns.Count);
            Assert.AreEqual("Narrative", dt.Columns[0].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(longData, dt.Rows[0][0]);
            }

        [Test]
        public void TestNullValue()
            {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Data><Matter><Number>1234</Number><Narrative /></Matter></Data>");
            var dt = GetArchetypeData.BuildDataTableStructure(xmlDoc);
            Assert.AreEqual(2, dt.Columns.Count);
            Assert.AreEqual("Number", dt.Columns[0].ColumnName);
            Assert.AreEqual("Narrative", dt.Columns[1].ColumnName);

            GetArchetypeData.FillDataTable(dt, xmlDoc);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("1234", dt.Rows[0]["Number"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["Narrative"]);
            }
        }
    }
