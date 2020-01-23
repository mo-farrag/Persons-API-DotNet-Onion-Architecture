You can use the attached test.postman_collection.json to import is in Postman and check the 3 API requests:
	1- POST /api/Persons for task 1
	2- POST /api/token/generate for task 2
	3- POST /api/token/verify for task 3
	

Task 1:
	is accessed using a POST request on /api/Persons, it will validate the request and add the person to database. Avatar is attached as base64 string.

Task 2:
	as the Password field is not clear in the requirment document about its usage and I didn't clearification on my inquiry email that I sent, I assumed that that Password field is for encrypting the auth token, so I used it that way.

Task 3: 
	as the Status field and the Status object response are not clear in the requirment document about its usage, I assummed that the status object is a verifiction about the passed token, so I assumed that it has 3 values based on the token status( valid, expired or not exists).
	I got confused between having the status as a parameter and returning the status in the response, so I ignored the status parameter and make the end-point expect phone and token only and return the status in the response.
	
	so here I'm verifying the passed token and phone against the data in the AuthToken table and return the status as following:
		- if the phone or the token are not exists in the table, I return status = not exists.
		- if the phone and the token are exists in the table, I check the Expiry date assigned to it and I return status = expired if it's less than Now.
		- els I return status = valid.