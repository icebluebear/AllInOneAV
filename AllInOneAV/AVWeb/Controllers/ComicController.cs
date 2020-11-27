using AVWeb.Filter;
using AVWeb.Models;
using Model.MangaModel;
using Model.WebModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService;

namespace AVWeb.Controllers
{
    [RoutePrefix("comic")]
    public class ComicController : ApiController
    {
        [Base]
        [Route("getmangasource")]
        [HttpGet]
        public MangaSourceVM GetMangaSource()
        {
            MangaSourceVM ret = new MangaSourceVM();
            ret.Sources = new List<MangaSourceItem>();

            foreach (MangaCategorySourceType st in Enum.GetValues(typeof(MangaCategorySourceType)))
            {
                ret.Sources.Add(new MangaSourceItem
                {
                    SourceName = st.ToString(),
                    SourceType = (int)st
                });
            }

            ret.MsgCode = VMCode.Success;

            return ret;
        }

        [Base]
        [Route("getmanagacategory")]
        [HttpGet]
        public MangaCategoryVM GetManagaCategory(MangaCategorySourceType sourceType)
        {
            var ret = MangaService.GetMangaCategory(sourceType);

            return ret;
        }

        [Base]
        [Route("getmangacategorylist")]
        [HttpGet]
        public MangaCategoryListVM GetMangaCategoryList(MangaCategorySourceType sourceType, string category, int page)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = "";
            }

            var model = MangaService.GetMangaCategoryList(sourceType, category, page);

            return model;
        }

        [Base]
        [Route("getmangadetail")]
        [HttpGet]
        public MangaDetailVM GetMangaDetail(MangaCategorySourceType sourceType, string path)
        {
            var model = MangaService.GetMangaDetail(sourceType, path);
            return model;
        }

        [Base]
        [Route("getmangapic")]
        [HttpGet]
        public MangaPicturesVM GetMangaPic(MangaCategorySourceType sourceType, string path)
        {
            var model = MangaService.GetMangaPic(sourceType, path);
            return model;
        }
    }
}
