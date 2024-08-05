
export async function postData(url, method, data) {
  return await fetch(url, {
    method: method,
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  }).then(
    res => { return res.json() })
    .catch(err => {
      console.log("something went wrong", err);
    });
}
export async function postFormData(url, method, data) {
  return await fetch(url, {
    body: data,
    method: method
  }).then(
    res => { return res.json() })
    .catch(err => {
      console.log("something went wrong", err);
    });
}
export async function getData(url, method) {
  return await fetch(url, {
    method: method,
  }).then(
    res => { return res.json() })
    .catch(err => {
      console.log("something went wrong", err);
    });
}
export async function delData(url, method) {
  return await fetch(url, {
    method: method,
  }).then(
    res => { return res.status })
    .catch(err => {
      console.log("something went wrong", err);
    });
}