using Microsoft.EntityFrameworkCore;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.Model.Models;
using SwiftCode.BBS.Repositories;
using SwiftCode.BBS.Repositories.BASE;
using SwiftCode.BBS.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SwiftCode.BBS.Tests
{
    public class ArticleServicesShould
    {
        private readonly DbContextOptions<SwiftCodeBbsContext> _dbOptions;

        public ArticleServicesShould()
        {
            _dbOptions = new DbContextOptionsBuilder<SwiftCodeBbsContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;
        }

        [Fact]
        public async Task AddNewItemAsIncompleteForAdditionalAsync()
        {
            using (var context = new SwiftCodeBbsContext(_dbOptions))
            {
                // Arrange
                var repository = new BaseRepository<Article>(context);
                var articlRepository = new ArticleRepository(context);
                ArticleServices service = new ArticleServices(repository, articlRepository);

                var fakeArticle = new Article
                {
                    Id = 1,
                    Tag = "test",
                    Title = "test",
                    Content = "test",
                    CreateUser = new UserInfo() { },
                    CreateUserId = 1,
                };

                // Act����
                // ��¼����ǰ
                await service.AdditionalItemAsync(fakeArticle, true, 3);

                // ��ʼ��������
                // Act����
                var itemsInDatabase = await context.Articles.CountAsync();
                // Assert
                // ����1���ж��ڵ�ǰ�������У��Ƿ���ӵ�������1
                Assert.Equal(1, itemsInDatabase);
                // ����2�����±���
                var item = await context.Articles.FirstAsync();
                Assert.Equal("test", item.Title);
                // ����3���Ƿ�Ϊ��¼����ǰ����Ȼ���ֵ��������ֵС��һ��
                var difference = DateTime.Now.AddDays(-3) - item.CreateTime;
                Assert.True(difference < TimeSpan.FromSeconds(1));
            }

        }
    }
}
