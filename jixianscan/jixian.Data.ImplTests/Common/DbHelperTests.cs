using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jixian.Data.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jixian.Entity;
namespace jixian.Data.Impl.Tests
{
    [TestClass()]
    public class DbHelperTests
    {
        [TestMethod()]
        public void ExcuteTest()
        {
            string sql = @"




";

            string sql1 = @" 
CREATE TABLE CustomerInfo
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      CustomerName NVARCHAR(50) NOT NULL ,
      TelPhone NVARCHAR(11) NULL ,
      Email NVARCHAR(30) NULL ,
      Address NVARCHAR(200) NULL
    );
CREATE TABLE PrintType
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      PrintType NVARCHAR(10) NOT NULL ,
      DefaultUnitPrice DECIMAL(18, 2) NOT NULL
    );
CREATE TABLE CustomerPrice
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      CustomerId INT NOT NULL ,
      PrintTypeId INT NOT NULL ,
      UnitPrice DECIMAL(18, 2) NOT NULL
    );
CREATE TABLE CustomerTradeInfo
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      CustomerId INT NOT NULL ,
      AccountType INT NOT NULL ,
      AccountNo VARCHAR(50) NOT NULL ,
      Holder NVARCHAR(20) NOT NULL
    );
CREATE TABLE OrderInfo
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      CustomerId INT NOT NULL ,
      CustomerName NVARCHAR(30) NOT NULL ,
      PrintDate DATE NOT NULL ,
      PrintType NVARCHAR(10) NOT NULL ,
      Length FLOAT NOT NULL ,
      Width FLOAT NOT NULL ,
      Quantity INT NOT NULL ,
      UnitPrice DECIMAL(18, 2) NOT NULL ,
      AreaSize FLOAT NOT NULL ,
      TotalAmount DECIMAL(18, 2) NOT NULL ,
      FileName NVARCHAR(50) NOT NULL ,
      FileFullName NVARCHAR(200) NOT NULL ,
      OrderStatus INT NOT NULL ,
      SettlementId INT NOT NULL
    );
CREATE TABLE AccountType
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      Type NVARCHAR(50) NOT NULL
    );
CREATE TABLE MyTradeAccount
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      AccountType INT NOT NULL ,
      AccountTypeName NVARCHAR(20) NOT NULL ,
      AccountNo NVARCHAR(100) NOT NULL ,
      Holder NVARCHAR(10) NOT NULL
    );
CREATE TABLE SettlementInfo
    (
      Id INTEGER PRIMARY KEY  autoincrement
                 NOT NULL ,
      SettlementDate DATE NOT NULL ,
      CustomerId INT NOT NULL ,
      SettlementType INT NOT NULL ,
      AmountReceivable DECIMAL(18, 2) NOT NULL ,
      AmountReceived DECIMAL(18, 2) NOT NULL ,
      Payee NVARCHAR(10) NOT NULL ,
      ReceiveAccount NVARCHAR(30) NOT NULL ,
      ReceiveAccountType NVARCHAR(10) NOT NULL ,
      Payer NVARCHAR(10) NOT NULL ,
      PaidAccount NVARCHAR(30) NOT NULL ,
      PaidAccountType NVARCHAR(10) NULL
    );";

            var db = DbHelper.GetInstant(@"E:\\jixianDB", "jixian.db").Excute(sql1);

        }

        [TestMethod()]
        public void ExcuteTest1()
        {
            var sql = " insert into AccountType (Type) values (@Type) ";
            List<AccountType> list = new List<AccountType>()
            {
                new AccountType(){ Type="支付宝"},
                new AccountType(){ Type="微信"},
                new AccountType(){ Type="中国银行"},
                new AccountType(){ Type="建设银行"},
            };
            var result = DbHelper.GetInstant(@"E:\\jixianDB", "jixian.db").Excute(sql, list);
            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        public void QueryListTest()
        {
            var sql = " select *  from AccountType ";
            var result = DbHelper.GetInstant(@"E:\\jixianDB", "jixian.db").QueryList<AccountType>(sql);
            var result2 = DbHelper.GetInstant(@"E:\\jixianDB", "jixian.db").QueryFirst<AccountType>(sql);
            Assert.AreNotEqual(null, result);
        }
    }
}
