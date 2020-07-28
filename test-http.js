const http = require('http');
const utils = require('util');
const os = require('os');
const fs = require('fs');
const path = require('path');

function query(params) {
  return new Promise(resolve => {
    http.get(`http://127.0.0.1:9999/?${new URLSearchParams(params).toString()}`,
      res => {
        res.on('data', chunk => {
          let str = chunk.toString();
          try {
            str = JSON.parse(str);
          } catch (error) {
            console.log();
            console.log(str);
            console.log('----');
            console.log(error);
            console.log();
          }
          resolve(str);
        });
      })
  });
}

function state(cb) {
  return query({
    cmd: 'state',
  });
}

function start() {
  query({
    cmd: 'start',
    host: '192.168.1.3',
    // file: path.join(os.homedir(), 'Desktop/plu.txt'),
    file: path.join(__dirname, './plu.txt'),
  }).then(json => {
    if (json.code === 0) {
      const t = setInterval(() => {
        query({ cmd: 'state' }).then(json => {
          if (json.data.code === 0) {
            clearTimeout(t);
            console.log('完成');
          } else if (json.data.code === 1) { // 进行中
            console.log(json.data);
          } else if (json.data.code === -1) { // 准备中
            console.log(json.data);
          } else { // 报错
            clearTimeout(t);
            console.log(json);
          }
        });
      }, 400);
    };
  });
}

start();

