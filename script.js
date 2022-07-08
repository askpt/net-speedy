import grpc from 'k6/net/grpc';
import http from 'k6/http';
import { check, sleep } from 'k6';

const client = new grpc.Client();
client.load(['Protos'], 'greet.proto');

export default () => {
    doGrpcTest();
    doHttpTest();
    sleep(1);
};

function doGrpcTest() {
    client.connect('localhost:5000', {
        plaintext: true
    });

    const data = { name: 'Bert' };
    const response = client.invoke('greet.Greeter/SayHelloAgain', data);

    check(response, {
        'status is OK': (r) => r && r.status === grpc.StatusOK,
    });

    // console.log(JSON.stringify(response.message));

    client.close();
}

function doHttpTest() {
    const response = http.get('http://localhost:5001/v1/greeter/Bert');

    check(response, {
        'status is 200': (r) => r && r.status === 200
    });

    // console.log(JSON.stringify(response.body));
}
