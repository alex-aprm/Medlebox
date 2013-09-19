
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Reflection;
using RefactorThis.EFExtensions;
using AutoMapper;
using Medlebox.Models;
using System.ComponentModel;


//TEST
namespace Medlebox.DAL
{
    public partial class MedleboxDAL
    {
        public User CurrentUser { get; set; }
        MedleboxDB db;
        public MedleboxDAL(string WeathertopConnection)
        {
            this.db = new MedleboxDB(WeathertopConnection);
            //God.GiveMeMaps(); //
        }
       public void Dispose()
        {
            this.db.Dispose();
        }
    }

 }