IMAGE_NAME="websocker-server"
PORT=80

sudo docker build --build-arg ENVIRONMENT=dev -t $IMAGE_NAME .
sudo docker rm -f $IMAGE_NAME || true
sudo docker run -d --name $IMAGE_NAME -p $PORT:80 $IMAGE_NAME
