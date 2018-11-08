import pymssql
try:

    conn = pymssql.connect(server="(localdb)\\mssqllocaldb",database="IssueStore")
    cursor = conn.cursor()
    cursor.execute("SELECT VERSION()")
except Exception as e:
    raise
finally:
    pass