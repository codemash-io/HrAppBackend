using HrApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class ESignTests
    {
        IESignatureService eSignService;
        IESignRepository eSignRepository;
        IFileRepository fileRepository;

        [SetUp]
        public void Setup()
        {
            fileRepository = new FileRepository();
            eSignRepository = new ESignRepository();
            eSignService = new ESingnatureService()
            {
                FileRepo = fileRepository,
                SignitureRepo = eSignRepository
            };

        }

        [Test]
        public async Task Sign()
        {

            await eSignService.OnPostAsync();
        }

       
    }
}