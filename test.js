const http = require('http');
const utils = require('util');
const os = require('os');
const fs = require('fs');
const path = require('path');
const { resolve } = require('path');

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
  query({
    cmd: 'state',
  }).then(json => {
    cb(json);
    // console.log(json);
  });
}

function start() {
  query({
    cmd: 'start',
    host: '192.168.1.3',
    file: path.join(os.homedir(), 'Desktop/plu.txt'),
  }).then(json => {
    if (json.code === 0) {
      const t = setInterval(() => {
        state(j => {
          const { data } = j;
          console.log(data);
          if (data.code === 0) {
            clearTimeout(t);
            console.log(完成);
          } else if (data.code > 1) {
            clearTimeout(t);
            console.log('报错');
          }
        })
      }, 400);
    };
  });
}

start();

