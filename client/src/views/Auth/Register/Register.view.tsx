import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface RegisterViewPorps extends RouteComponentProps<{ uid: number }> {

}

class RegisterView extends React.PureComponent<RegisterViewPorps>{

}

export default RegisterView

