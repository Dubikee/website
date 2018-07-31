import * as React from "react";
import "./Home.view.styl";
import MainLayout from "../../containers/Main/Main.layout";
import { Row, Col, Form, Checkbox, Button, Input, Icon } from "antd";
class HomeView extends React.Component {
    state = {
        uid: "",
        pwd: "",
        inputLeft: 0
    };
    render() {
        return (
            <div className="home">
                <Row className="form-row" type="flex" justify="center">
                    <Col
                        xs={18}
                        sm={12}
                        md={10}
                        lg={8}
                        xl={6}
                        xxl={4}
                        className='form-col'
                    >
                    <h1>LOGIN</h1>
                        <Form className="login-form">
                            <Form.Item>
                                <Input
                                    prefix={
                                        <Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />
                                    }
                                    placeholder="Account"
                                />
                            </Form.Item>
                            <Form.Item>
                                <Input
                                    prefix={
                                        <Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />
                                    }
                                    type="password"
                                    placeholder="Password"
                                />
                            </Form.Item>
                            <Form.Item className="form-item-last">
                                <Checkbox>Remember me</Checkbox>
                                <br/>
                                <Button
                                    type="primary"
                                    htmlType="submit"
                                    className="login-form-button"
                                >
                                    Log in
                                </Button>
                            </Form.Item>
                        </Form>
                    </Col>
                </Row>
            </div>
        );
    }
}

export default MainLayout(HomeView);
