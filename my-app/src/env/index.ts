const REMOTE_HOST_NAME: string = process.env.REACT_APP_BASE_URL as string;

const APP_ENV = {
    REMOTE_HOST_NAME: REMOTE_HOST_NAME,
    IMAGE_PATH: `${REMOTE_HOST_NAME}/images/`
};

export { APP_ENV };