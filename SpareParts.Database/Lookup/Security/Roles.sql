EXEC LookupScript N'
	{
	    "table": "Security.Roles",
        "records": [
            {
                "RoleID": 1,
                "RoleName": "Admin",
                "RoleCreationDate": "2025-12-13T00:00:00.000"
            },
            {
                "RoleID": 2,
                "RoleName": ":User",
                "RoleCreationDate": "2025-12-13T00:00:00.000"
            },
            {
                "RoleID": 3,
                "RoleName": "Shop Owner",
                "RoleCreationDate": "2025-12-13T00:00:00.000"
            }
        ]
	}
';