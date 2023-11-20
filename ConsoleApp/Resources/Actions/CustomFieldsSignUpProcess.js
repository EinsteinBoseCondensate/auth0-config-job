const protocolsBindedToNoRedirectBehavior = [
    'oauth2-password',
    'oauth2-refresh-token'
];

const isNullOrEmptyOrUndefined = field => field === null || field === '' || field === undefined;

const commonHandler = async (event, api) => {  
    const doesProtocolNotAllowRedirection = protocolsBindedToNoRedirectBehavior.includes(event.transaction?.protocol);
  
    const needToFillAdditionalData = isNullOrEmptyOrUndefined(event.user.given_name) || isNullOrEmptyOrUndefined(event.user.family_name);
  
    if (!needToFillAdditionalData || doesProtocolNotAllowRedirection)
      return;
  
    const token = api.redirect.encodeToken({
      secret: event.secrets.JwtSigningKey,
      payload: {
        firstName: event.user.given_name,
        lastName: event.user.family_name
      },
    });
  
    api.redirect.sendUserTo(event.secrets.CustomFieldsSiteUrl, {
      query: { session_token: token }
    });
  };
  
  
  /**
  * Handler that will be called during the execution of a PostLogin flow.
  *
  * @param {Event} event - Details about the user and the context in which they are logging in.
  * @param {PostLoginAPI} api - Interface whose methods can be used to change the behavior of the login.
  */
  exports.onExecutePostLogin = commonHandler
  
  
  
  /**
  * Handler that will be invoked when this action is resuming after an external redirect. If your
  * onExecutePostLogin function does not perform a redirect, this function can be safely ignored.
  *
  * @param {Event} event - Details about the user and the context in which they are logging in.
  * @param {PostLoginAPI} api - Interface whose methods can be used to change the behavior of the login.
  */
  exports.onContinuePostLogin = commonHandler
  