import grpc from 'k6/net/grpc';
import { check, sleep } from 'k6';

const client = new grpc.Client();
client.load(['Protos'], 'greet.proto');

export default () => {
    client.connect('localhost:5000', {
        plaintext: true
    });

    const data = { name: 'Bert' };
    const response = client.invoke('greet.Greeter/SayHelloAgain', data);

    check(response, {
        'status is OK': (r) => r && r.status === grpc.StatusOK,
    });

    console.log(JSON.stringify(response.message));

    client.close();
    sleep(1);
};
