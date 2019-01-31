using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISV.Services
{
    public interface IGithubService
    {
        Task<Issue> GetGithubIssueAsync(string issueFullLink);
    }

    public class GithubService : IGithubService
    {
        public Credentials TokenAuth { get; set; }

        public GitHubClient Client { get; set; }

        public GithubService()
        {
            Client = new GitHubClient(new ProductHeaderValue(this.GetType().Name));
            var access_token = Environment.GetEnvironmentVariable("PERSONAL_ACCESS_TOKEN");
            if(!string.IsNullOrEmpty(access_token))
            {
                TokenAuth = new Credentials(access_token);
                Client.Credentials = TokenAuth;
            }
        }

        public async Task<Issue> GetGithubIssueAsync(string issueFullLink)
        {
            if (!string.IsNullOrEmpty(issueFullLink))
            {
                //https://github.com/Microsoft/PTVS/issues/4785
                var result = issueFullLink.Trim().TrimEnd('/').Split('/');
                return await Client.Issue.Get(result[result.Length - 4], result[result.Length - 3], Convert.ToInt32(result[result.Length - 1]));
            }

            return null;
        }
    }
}
