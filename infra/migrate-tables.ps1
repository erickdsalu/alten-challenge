echo "Starting table creation"
aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Configurations --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 --no-paginate

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Customers --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Reservations --attribute-definitions AttributeName=RoomId,AttributeType=S AttributeName=ReservationId,AttributeType=S AttributeName=CustomerId,AttributeType=S --key-schema AttributeName=RoomId,KeyType=HASH AttributeName=ReservationId,KeyType=RANGE --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 --global-secondary-indexes "[    {        \"IndexName\": \"UserReservationIndex\",        \"KeySchema\": [            {\"AttributeName\":\"CustomerId\",\"KeyType\":\"HASH\"},            {\"AttributeName\":\"ReservationId\",\"KeyType\":\"RANGE\"}        ],        \"Projection\": {            \"ProjectionType\":\"ALL\"        },        \"ProvisionedThroughput\": {            \"ReadCapacityUnits\": 1,            \"WriteCapacityUnits\": 1        }    }]"

aws dynamodb create-table --endpoint-url http://localhost:8000 --table-name Rooms --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
echo "Finished table creation"

echo "Starting migrations"

echo "Migrating configurations"

aws dynamodb put-item --table-name Configurations --item file://tables/Configurations.json --endpoint-url http://localhost:8000

echo "Migrating rooms"

aws dynamodb put-item --table-name Rooms --item file://tables/Rooms.json --endpoint-url http://localhost:8000

echo "Finished migrations"

pause