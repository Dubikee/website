from flask import Flask, jsonify, request

app = Flask(__name__)


@app.route('/')
def ping():
	return "Pong"


@app.route('/api/account/login', methods=['Post'])
def accountLogin():
	uid = request.form['uid']
	pwd = request.form['pwd']
	if uid == 'luokun' and pwd == 'abcd':
		return jsonify({'code': 0, 'jwt': 'abcd1234'})
	else:
		return jsonify({'code': 1, 'jwt': None})


app.run(port=5060, debug=True)
