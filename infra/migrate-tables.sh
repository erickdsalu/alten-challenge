aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Configuration --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 --no-paginate

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Customer --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Reservation --attribute-definitions AttributeName=Id,AttributeType=S AttributeName=RoomId,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH AttributeName=RoomId,KeyType=RANGE --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Room --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1

echo "Finished executing table and migrations"
pause