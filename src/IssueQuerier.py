from github import *

g = Github()
repo_instances = {}
repos = [["dotnet/coreclr",20629],["dotnet/coreclr",20626],["shimingsg/Myixy",1]]
for rp in repos:
	if rp[0] in repo_instances:
		pass
	else:
		repo_instances[rp[0]]=g.get_repo(rp[0])
	try:
		issue = repo_instances[rp[0]].get_issue(number=rp[1])
		print(issue.title)
	except UnknownObjectException as e:
		print(rp[1] + "except:" + e.data["message"])
	except Exception as e:
		print(rp[1] + "except:" + e.data["message"])

#issue = repo.get_issue(number=11)
#github.GithubException.UnknownObjectException: 404 {'message': 'Not Found', 'documentation_url': 'https://developer.github.com/v3/issues/#get-a-single-issue'}




