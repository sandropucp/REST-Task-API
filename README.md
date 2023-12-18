# REST Task API

I will use this project to show all the steps and architectural decisions to develop a robust and scalable solution. As a database, I will use Postgresql running in a Docker container.. You can find more details in this
[medium blog](https://medium.com/@sandropucp/natural-language-processing-nlp-practice-with-python-07320d47890f).

1. Task creation, editing, and deletion.
2. Task categorization or prioritization.
3. Slug Generation
4. Logging 
5. Validation
6. Custom Errors
7. Response with Codes to indicate status
8. Cancellation Tokens to cancel requests

To run the database, you can use the following command:

```bash
docker compose up
```

To stop the database, you can use the following command:

```bash
docker compose down
```