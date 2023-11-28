# MyMicroservice
Run `docker compose up` to setup
Make sure you have dotnet ef version 8.x.x

Run http://localhost:5050/ to get to the db management dashboard
Login: admin@admin.com - Password: root

To connect to the db
- Select Add new server
- Type in any name
- In the connection tab, enter Host name as 172.19.0.2, Username is root, password is root 
- If the host name isn't correct, use docker command to retrieve the IP address of the db container
- `docker ps` to get the list of docker container, `docker inspect <container_id>` where `<container_id>` is the id of the db, and get the IP address under Networks