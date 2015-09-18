﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
// Apache License 2.0
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace System.IdentityModel.Tokens
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    public static class LogMessages
    {
        #pragma warning disable 1591
        // general
        public const string IDX10000 = "IDX10000: The parameter '{0}' cannot be a 'null' or an empty object.";
        public const string IDX10001 = "IDX10001: The property value '{0}' cannot be a 'null' or an empty object.";
        public const string IDX10002 = "IDX10002: The parameter '{0}' cannot be 'null' or a string containing only whitespace.";

        // properties, configuration 
        public const string IDX10100 = "IDX10100: ClockSkew must be greater than TimeSpan.Zero. value: '{0}'";
        public const string IDX10101 = "IDX10101: MaximumTokenSizeInBytes must be greater than zero. value: '{0}'";
        public const string IDX10102 = "IDX10102: NameClaimType cannot be null or whitespace.";
        public const string IDX10103 = "IDX10103: RoleClaimType cannot be null or whitespace.";
        public const string IDX10104 = "IDX10104: TokenLifetimeInMinutes must be greater than zero. value: '{0}'";
        public const string IDX10105 = "IDX10105: NonceLifetime must be greater than zero. value: '{0}'";
        public const string IDX10106 = "IDX10106: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'";
        public const string IDX10107 = "IDX10107: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'";
        public const string IDX10108 = "IDX10108: The address specified is not valid as per HTTPS scheme. Please specify an https address for security reasons. If you want to test with http address, set the RequireHttps property  on IDocumentRetriever to false. address: '{0}'";

        // token validation
        public const string IDX10200 = "IDX10200: Support for ValidateToken(string, TokenValidationParameters) requires a handler to implement ISecurityTokenValidator, none of the SecurityTokenHandlers did.";
        public const string IDX10201 = "IDX10201: None of the the SecurityTokenHandlers could read the 'securityToken': '{0}'.";
        public const string IDX10202 = "IDX10202: SamlToken.Assertion is null, can not create an identity.";
        public const string IDX10203 = "IDX10203: Unable to create ClaimsIdentity. Issuer is null or whitespace.";
        public const string IDX10204 = "IDX10204: Unable to validate issuer. validationParameters.ValidIssuer is null or whitespace AND validationParameters.ValidIssuers is null.";
        public const string IDX10205 = "IDX10205: Issuer validation failed. Issuer: '{0}'. Did not match: validationParameters.ValidIssuer: '{1}' or validationParameters.ValidIssuers: '{2}'.";
        public const string IDX10207 = "IDX10207: Unable to validate audience, o audiences to .";
        public const string IDX10208 = "IDX10208: Unable to validate audience. validationParameters.ValidAudience is null or whitespace and validationParameters.ValidAudiences is null.";
        public const string IDX10209 = "IDX10209: token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.";
        public const string IDX10210 = "IDX10210: SamlToken.Assertion.Issuer is null, can not create an identity.";
        public const string IDX10211 = "IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace";
        public const string IDX10212 = "IDX10212: {0} can only validate tokens of type {1}.";
        public const string IDX10213 = "IDX10213: SecurityTokens must be signed. SecurityToken: '{0}'.";
        public const string IDX10214 = "IDX10214: Audience validation failed. Audiences: '{0}'. Did not match:  validationParameters.ValidAudience: '{1}' or validationParameters.ValidAudiences: '{2}'";
        public const string IDX10215 = "IDX10215: Audience validation failed. Audiences passed in was null";
        public const string IDX10216 = "IDX10216: Lifetime validation failed. 'NotBefore' preceeds the current time: '{0}', ClockSkew (InSeconds): '{1}', notbefore: '{2}'";
        public const string IDX10217 = "IDX10217: Lifetime validation failed. 'NotOnOrAfter' is after the current time: '{0}', ClockSkew (InSeconds): '{1}', notbefore: '{2}'";
        public const string IDX10218 = "IDX10218: OneTimeUse is not supported";
        public const string IDX10219 = "IDX10219: ProxyRestriction is not supported";
        public const string IDX10220 = "IDX10220: Jwks_Uri must be an absolute uri. Was: ";
        public const string IDX10221 = "IDX10221: Unable to create claims from securityToken, 'issuer' is null or empty.";
        public const string IDX10222 = "IDX10222: Lifetime validation failed. The token is not yet valid.\nValidFrom: '{0}'\nCurrent time: '{1}'.";
        public const string IDX10223 = "IDX10223: Lifetime validation failed. The token is expired.\nValidTo: '{0}'\nCurrent time: '{1}'.";
        public const string IDX10224 = "IDX10224: Lifetime validation failed. The NotBefore: '{0}' is after Expires: '{1}'.";
        public const string IDX10225 = "IDX10225: Lifetime validation failed. The token is missing an Expiration Time.\nTokentype: '{0}'.";
        public const string IDX10226 = "IDX10226: '{0}' can only write SecurityTokens of type: '{1}', 'token' type is: '{2}'.";
        public const string IDX10227 = "IDX10227: TokenValidationParameters.TokenReplayCache is not null, indicating to check for token replay but the security token has no expiration time: token '{0}'.";
        public const string IDX10228 = "IDX10228: The securityToken has previously been validated, securityToken: '{0}'.";
        public const string IDX10229 = "IDX10229: TokenValidationParameters.TokenReplayCache was unable to add the securityToken: '{0}'.";
        public const string IDX10230 = "IDX10230: Lifetime validation failed. Delegate returned false, securitytoken: '{0}'.";
        public const string IDX10231 = "IDX10231: Audience validation failed. Delegate returned false, securitytoken: '{0}'.";
        public const string IDX10232 = "IDX10232: IssuerSigningKey validation failed. Delegate returned false, securityKey: '{0}'.";
        public const string IDX10233 = "IDX10233: ValidateAudience property on ValidationParamaters is set to false. Exiting without validating the audience.";
        public const string IDX10234 = "IDX10244: Audience Validated.Audience: '{0}'";
        public const string IDX10235 = "IDX10235: ValidateIssuer property on ValidationParamaters is set to false. Exiting without validating the issuer.";
        public const string IDX10236 = "IDX10236: Issuer Validated.Issuer: '{0}'";
        public const string IDX10237 = "IDX10237: ValidateIssuerSigningKey property on ValidationParamaters is set to false. Exiting without validating the issuer signing key.";
        public const string IDX10238 = "IDX10238: ValidateLifetime property on ValidationParamaters is set to false. Exiting without validating the lifetime.";
        public const string IDX10239 = "IDX10239: Lifetime of the token is validated.";
        public const string IDX10240 = "IDX10240: No token replay is detected.";
        public const string IDX10241 = "IDX10241: Security token validated. token: '{0}'.";
        public const string IDX10242 = "IDX10242: Security token: '{0}' has a valid signature.";
        public const string IDX10243 = "IDX10243: Reading issuer signing keys from validaiton parameters.";
        public const string IDX10244 = "IDX10244: Issuer is null or empty. Using runtime default for creating claims.";
        public const string IDX10245 = "IDX10245: Creating claims identity from the validated token: '{0}'.";

        // protocol validation
        public const string IDX10300 = "IDX10300: The hash claim: '{0}' in the id_token did not validate with against: '{1}', algorithm: '{2}'.";
        public const string IDX10301 = "IDX10301: The algorithm: '{0}' specified in the jwt header was unable to create a .Net hashAlgorithm. See inner exception for details.\nPossible solution is to ensure that the algorithm specified in the 'JwtHeader' is understood by .Net. You can make additions to the OpenIdConnectProtocolValidationParameters.AlgorithmMap to map algorithms from the 'Jwt' space to .Net. In .Net you can also make use of 'CryptoConfig' to map algorithms.";
        public const string IDX10302 = "IDX10302: The algorithm: '{0}' specified in the jwt header is not suported.";
        public const string IDX10303 = "IDX10303: Validating hash of OIDC protocol message. Expected: '{0}'.";
        public const string IDX10304 = "IDX10304: Validating 'c_hash' using id_token and code.";
        public const string IDX10305 = "IDX10305: OpenIdConnectProtocolValidationContext.ProtocolMessage.Code is null, there is no 'code' in the OpenIdConnect Response to validate.";
        public const string IDX10306 = "IDX10306: The 'c_hash' claim was not a string in the 'id_token', but a 'code' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        public const string IDX10307 = "IDX10307: The 'c_hash' claim was not found in the id_token, but a 'code' was in the OpenIdConnectMessage, id_token: '{0}'";
        public const string IDX10308 = "IDX10308: 'Azp' claim exist in the 'id_token' but 'ciient_id' is null. Cannot validate the 'azp' claim.";
        public const string IDX10309 = "IDX10309: Validating 'at_hash' using id_token and token.";
        public const string IDX10310 = "IDX10310: OpenIdConnectProtocolValidationContext.ProtocolMessage.token is null, there is no 'token' in the OpenIdConnect Response to validate.";
        public const string IDX10311 = "IDX10311: The 'at_hash' claim was not a string in the 'id_token', but a 'token' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        public const string IDX10312 = "IDX10312: The 'at_hash' claim was not found in the 'id_token', but a 'token' was in the OpenIdConnectMessage, 'id_token': '{0}'.";
        public const string IDX10313 = "IDX10313: The id_token: '{0}' is not valid. Please see exception for more details. exception: '{1}'.";
        public const string IDX10314 = "IDX10314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.";
        public const string IDX10315 = "IDX10315: RequireAcr is 'true' (default is 'false') but jwt.PayLoad.Acr is 'null or whitespace', jwt: '{0}'.";
        public const string IDX10316 = "IDX10316: RequireAmr is 'true' (default is 'false') but jwt.PayLoad.Amr is 'null or whitespace', jwt: '{0}'.";
        public const string IDX10317 = "IDX10317: RequireAuthTime is 'true' (default is 'false') but jwt.PayLoad.AuthTime is 'null or whitespace', jwt: '{0}'.";
        public const string IDX10318 = "IDX10318: RequireAzp is 'true' (default is 'false') but jwt.PayLoad.Azp is 'null or whitespace', jwt: '{0}'.";
        public const string IDX10319 = "IDX10319: Validating the nonce claim found in the id_token.";
        public const string IDX10320 = "IDX10320: RequireNonce is '{0}' but OpenIdConnectProtocolValidationContext.Nonce is null. A nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.";
        public const string IDX10321 = "IDX10321: The 'nonce' found in the jwt token did not match the expected nonce.\nexpected: '{0}'\nfound in jwt: '{1}'.\njwt: '{2}'.";
        public const string IDX10322 = "IDX10322: RequireNonce is false, validationContext.Nonce is null and there is no 'nonce' in the OpenIdConnect Response to validate.";
        public const string IDX10323 = "IDX10323: RequireNonce is '{0}', the OpenIdConnect request contained nonce but the jwt does not contain a 'nonce' claim. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.\n jwt: '{1}'.";
        public const string IDX10324 = "IDX10324: The 'nonce' has expired: '{0}'. Time from 'nonce': '{1}', Current Time: '{2}'. NonceLifetime is: '{3}'.";
        public const string IDX10325 = "IDX10325: The 'nonce' did not contain a timestamp: '{0}'.\nFormat expected is: <epochtime>.<noncedata>.";
        public const string IDX10326 = "IDX10326: The 'nonce' timestamp could not be converted to a positive integer (greater than 0).\ntimestamp: '{0}'\nnonce: '{1}'.";
        public const string IDX10327 = "IDX10327: The 'nonce' timestamp: '{0}', could not be converted to a DateTime using DateTime.FromBinary({0}).\nThe value must be between: '{1}' and '{2}'.";
        public const string IDX10328 = "IDX10328: Generating nonce for openIdConnect message.";
        public const string IDX10329 = "IDX10329: RequireStateValidation is '{0}' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.";
        public const string IDX10330 = "IDX10330: RequireStateValidation is '{0}', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.";
        public const string IDX10331 = "IDX10331: The 'state' parameter in the message: '{0}', does not equal the 'state' in the context: '{1}'.";
        public const string IDX10332 = "IDX10332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against.";
        public const string IDX10333 = "IDX10333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate.";
        public const string IDX10334 = "IDX10334: Both 'id_token' and 'code' are null in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Authorization Endpoint. Cannot process the message.";
        public const string IDX10335 = "IDX10335: 'refresh_token' cannot be present in a response message received from Authorization Endpoint.";
        public const string IDX10336 = "IDX10336: Both 'id_token' and 'token' should be present in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Token Endpoint. Cannot process the message.";
        public const string IDX10337 = "IDX10337: OpenIdConnectProtocolValidationContext.UserInfoEndpointResponse is null, there is no OpenIdConnect Response to validate.";
        public const string IDX10338 = "IDX10338: Subject claim present in 'id_token': '{0}' does not match the claim received from UserInfo Endpoint: '{1}'.";
        public const string IDX10339 = "IDX10339: The 'id_token' contains multiple audiences but 'azp' claim is missing.";
        public const string IDX10340 = "IDX10340: The 'id_token' contains 'azp' claim but its value is not equal to Client Id. 'azp': '{0}'. clientId: '{1}.";
        public const string IDX10341 = "IDX10341: 'RequireStateValidation' = false, OpenIdConnectProtocolValidationContext.State is null and there is no 'state' in the OpenIdConnect response to validate.";


        // SecurityTokenHandler messages
        public const string IDX10400 = "IDX10400: The '{0}', can only process SecurityTokens of type: '{1}'. The SecurityToken received is of type: '{2}'.";
        public const string IDX10401 = "IDX10401: Expires: '{0}' must be after NotBefore: '{1}'.";

        // SignatureValidation
        public const string IDX10500 = "IDX10500: Signature validation failed. Unable to resolve SecurityKeyIdentifier: '{0}', \ntoken: '{1}'.";
        public const string IDX10501 = "IDX10501: Signature validation failed. Key tried: '{0}'.\ntoken: '{1}'";
        public const string IDX10502 = "IDX10502: Signature validation failed. Key tried: '{0}'.\nException caught:\n '{1}'.\ntoken: '{2}'";
        public const string IDX10503 = "IDX10503: Signature validation failed. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'";
        public const string IDX10504 = "IDX10504: Unable to validate signature, token does not have a signature: '{0}'";
        public const string IDX10505 = "IDX10505: Unable to validate signature. The 'Delegate' specified on TokenValidationParameters, returned a null SecurityKey.\nSecurityKeyIdentifier: '{0}'\nToken: '{1}'.";
        public const string IDX10506 = "IDX10506: Signature validation failed. The 'Delegate' specified on TokenValidationParameters returned null SecurityToken, token: '{0}'.";

        // Crypto Errors
        public const string IDX10600 = "IDX10600: '{0}' supports: '{1}' of types: '{2}' or '{3}'. SecurityKey received was of type: '{4}'.";
        public const string IDX10603 = "IDX10603: The '{0}' cannot have less than: '{1}' bits.";
        public const string IDX10611 = "IDX10611: AsymmetricSecurityKey.GetHashAlgorithmForSignature( '{0}' ) returned null.\nKey: '{1}'\nSignatureAlgorithm: '{0}'";
        public const string IDX10613 = "IDX10613: Cannot set the MinimumAsymmetricKeySizeInBitsForSigning to less than: '{0}'.";
        public const string IDX10614 = "IDX10614: AsymmetricSecurityKey.GetSignatureFormater( '{0}' ) threw an exception.\nKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.\nException:'{2}'.\nIf you only need to verify signatures the parameter 'willBeUseForSigning' should be false if the private key is not be available.";
        public const string IDX10615 = "IDX10615: AsymmetricSecurityKey.GetSignatureFormater( '{0}' ) returned null.\nKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.";
        public const string IDX10616 = "IDX10616: AsymmetricSecurityKey.GetSignatureDeformatter( '{0}' ) threw an exception.\nKey: '{1}'\nSignatureAlgorithm: '{0}, check to make sure the SignatureAlgorithm is supported.'\nException:'{2}'.";
        public const string IDX10617 = "IDX10617: AsymmetricSecurityKey.GetSignatureDeFormater( '{0}' ) returned null.\nKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.";
        public const string IDX10618 = "IDX10618: AsymmetricSecurityKey.GetHashAlgorithmForSignature( '{0}' ) threw an exception.\nAsymmetricSecurityKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.\nException: '{2}'.";
        public const string IDX10620 = "IDX10620: The AsymmetricSignatureFormatter is null, cannot sign data.  Was this AsymmetricSignatureProvider constructor called specifying setting parameter: 'willCreateSignatures' == 'true'?.";
        public const string IDX10621 = "IDX10621: This AsymmetricSignatureProvider has a minimum key size requirement of: '{0}', the AsymmetricSecurityKey in has a KeySize of: '{1}'.";
        public const string IDX10623 = "IDX10623: The KeyedHashAlgorithm is null, cannot sign data.";
        public const string IDX10624 = "IDX10624: Cannot sign 'input' byte array has length 0.";
        public const string IDX10625 = "IDX10625: Cannot verify signature 'input' byte array has length 0.";
        public const string IDX10626 = "IDX10626: Cannot verify signature 'signature' byte array has length 0.";
        public const string IDX10627 = "IDX10627: Cannot set the MinimumAsymmetricKeySizeInBitsForVerifying to less than: '{0}'.";
        public const string IDX10628 = "IDX10628: Cannot set the MinimumSymmetricKeySizeInBits to less than: '{0}'.";
        public const string IDX10629 = "IDX10629: The AsymmetricSignatureDeformatter is null, cannot sign data. If a derived AsymmetricSignatureProvider is being used, make sure to call the base constructor.";
        public const string IDX10630 = "IDX10630: The '{0}' for signing cannot be smaller than '{1}' bits.";
        public const string IDX10631 = "IDX10631: The '{0}' for verifying cannot be smaller than '{1}' bits.";
        public const string IDX10632 = "IDX10632: SymmetricSecurityKey.GetKeyedHashAlgorithm( '{0}' ) threw an exception.\nSymmetricSecurityKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.\nException: '{2}'.";
        public const string IDX10633 = "IDX10633: SymmetricSecurityKey.GetKeyedHashAlgorithm( '{0}' ) returned null.\n\nSymmetricSecurityKey: '{1}'\nSignatureAlgorithm: '{0}', check to make sure the SignatureAlgorithm is supported.";
        public const string IDX10634 = "IDX10634: KeyedHashAlgorithm.Key = SymmetricSecurityKey.GetSymmetricKey() threw.\n\nSymmetricSecurityKey: '{1}'\nSignatureAlgorithm: '{0}' check to make sure the SignatureAlgorithm is supported.\nException: '{2}'.";
        public const string IDX10635 = "IDX10635: Unable to create signature. '{0}' returned a null '{1}'. SecurityKey: '{2}', Algorithm: '{3}'";
        public const string IDX10636 = "IDX10636: SignatureProviderFactory.CreateForVerifying returned null for key: '{0}', signatureAlgorithm: '{1}'.";
        public const string IDX10637 = "IDX10637: the 'validationMode' is not supported '{0}'.  Supported values are: 'ChainTrust, PeerTrust, PeerOrChainTrust, None'.";
        public const string IDX10638 = "IDX10638: Cannot created the SignatureProvider, 'key.HasPrivateKey' is false, cannot create signatures. Key: {0}.";
        public const string IDX10639 = "IDX10639: Cannot created the SignatureProvider, the algorithm is not supported: '{0}'.";
        public const string IDX10640 = "IDX10640: Algorithm is not supported: '{0}'.";
        public const string IDX10641 = "IDX10641: Key is not supported: '{0}'.";
        public const string IDX10642 = "IDX10642: Creating signature using the input: '{0}'.";
        public const string IDX10643 = "IDX10643: Comparing the signature created over the input with the token signature: '{0}'.";
        public const string IDX10644 = "IDX10644: Creating raw signature using the signature provider.";
        public const string IDX10645 = "IDX10645: Creating raw signature using the signature credentials.";

        // JWT specific errors
        public const string IDX10700 = "IDX10700: Error found while parsing date time. The '{0}' claim has value '{1}' which is could not be parsed to an integer.\nInnerException: '{2}'.";
        public const string IDX10701 = "IDX10701: Error found while parsing date time. The '{0}' claim has value '{1}' does not lie in the valid range. \nInnerException: '{2}'.";
        public const string IDX10702 = "IDX10702: Jwt header type specified, must be '{0}' or '{1}'.  Type received: '{2}'.";
        public const string IDX10703 = "IDX10703: Unable to decode the '{0}': '{1}' as Base64url encoded string. jwtEncodedString: '{2}'.";
        public const string IDX10704 = "IDX10704: Cannot set inner IssuerTokenResolver to self.";
        public const string IDX10705 = "IDX10705: The SigningKeyIdentifier was of type: '{0}' and was expected to be encoded as a Base64UrlEncoded string. See inner exception for more details.";
        public const string IDX10706 = "IDX10706: '{0}' can only write SecurityTokens of type: '{1}', 'token' type is: '{2}'.";
        public const string IDX10707 = "IDX10707: '{0}' cannot read this xml: '{1}'. The reader needs to be positioned at an element: '{2}', within the namespace: '{3}', with an attribute: '{4}' equal to one of the following: '{5}', '{6}'.";
        public const string IDX10708 = "IDX10708: '{0}' cannot read this string: '{1}'.\nThe string needs to be in compact JSON format, which is of the form: '<Base64UrlEncodedHeader>.<Base64UrlEndcodedPayload>.<OPTIONAL, Base64UrlEncodedSignature>'.";
        public const string IDX10709 = "IDX10709: '{0}' is not well formed: '{1}'. The string needs to be in compact JSON format, which is of the form: '<Base64UrlEncodedHeader>.<Base64UrlEndcodedPayload>.<OPTIONAL, Base64UrlEncodedSignature>'.";
        public const string IDX10710 = "IDX10710: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'";
        public const string IDX10711 = "IDX10711: actor.BootstrapContext is not a string AND actor.BootstrapContext is not a JWT";
        public const string IDX10712 = "IDX10712: actor.BootstrapContext is null. Creating the token using actor.Claims.";
        public const string IDX10713 = "IDX10713: Creating actor value using actor.BootstrapContext(as string)";
        public const string IDX10714 = "IDX10714: Creating actor value using actor.BootstrapContext.rawData";
        public const string IDX10715 = "IDX10715: Creating actor value by writing the JwtSecurityToken created from actor.BootstrapContext";
        public const string IDX10716 = "IDX10716: Decoding token: '{0}' into header, payload and signature.";
        public const string IDX10717 = "IDX10717: Deserializing header: '{0}' from the token.";
        public const string IDX10718 = "IDX10718: Deserializing payload: '{0}' from the token.";
        public const string IDX10719 = "IDX10719: Token string length greater than maximum length allowed. Token string length: {0}";
        public const string IDX10720 = "IDX10720: Token string does not match the token format: header.payload.signature";
        public const string IDX10721 = "IDX10721: Creating payload and header from the passed parameters including issuer, audience, signing credentials and others.";
        public const string IDX10722 = "IDX10722: Creating security token from the header: '{0}', payload: '{1}' and raw signature: '{2}'.";
        public const string IDX10723 = "IDX10723: Adding the signature: '{0}' to the token";

        // configuration retrieval errors
        public const string IDX10800 = "IDX10800: JsonWebKeySet must have a 'Keys' element.";
        public const string IDX10801 = "IDX10801: Unable to create an RSA public key from the Exponent and Modulus found in the JsonWebKey: E: '{0}', N: '{1}'. See inner exception for additional details.";
        public const string IDX10802 = "IDX10802: Unable to create an X509Certificate2 from the X509Data: '{0}'. See inner exception for additional details.";
        public const string IDX10803 = "IDX10803: Unable to create to obtain configuration from: '{0}'.";
        public const string IDX10804 = "IDX10804: Unable to retrieve document from: '{0}'.";
        public const string IDX10805 = "IDX10805: Obtaining information from metadata endpoint: '{0}'";
        public const string IDX10806 = "IDX10806: Deserializing json string into json web keys.";
        public const string IDX10807 = "IDX10807: Adding signing keys into the configuration object.";
        public const string IDX10808 = "IDX10808: Deserializing json into OpenIdConnectConfiguration object: '{0}'.";
        public const string IDX10809 = "IDX10809: Serializing OpenIdConfiguration object to json string.";
        public const string IDX10810 = "IDX10810: Initializing an instance of OpenIdConnectConfiguration from a dictionary.";
        public const string IDX10811 = "IDX10811: Deserializing the string: '{0}' obtained from metadata endpoint into openIdConnectConfiguration object.";
        public const string IDX10812 = "IDX10812: Retrieving json web keys from: '{0}'.";
        public const string IDX10813 = "IDX10813: Deserializing json web keys: '{0}'.";
        public const string IDX10814 = "IDX10814: Cannot read file from the address: '{0}'. File does not exist.";

        // wsfederation messages
        public const string IDX10900 = "IDX10900: Building wsfederation message from query string: '{0}'.";
        public const string IDX10901 = "IDX10901: Building wsfederation message from uri: '{0}'.";

        // NotSupported Exceptions
        public const string IDX11000 = "IDX11000: This method is not supported to validate a 'saml2token' use the method: ValidateToken(String, TokenValidationParameters, out SecurityToken).";
        public const string IDX11001 = "IDX11001: This method is not supported to validate a 'samltoken' use the method: ValidateToken(String, TokenValidationParameters, out SecurityToken).";
        public const string IDX11002 = "IDX11002: This method is not supported to read a 'saml2token' use the method: ReadToken(XmlReader reader, TokenValidationParameters validationParameters).";
        public const string IDX11003 = "IDX11003: This method is not supported to read a 'samltoken' use the method: ReadToken(XmlReader reader, TokenValidationParameters validationParameters).";
        public const string IDX11004 = "IDX11004: Loading from Configuration is not supported use TokenValidationParameters to set validation parameters.";
        public const string IDX11005 = "IDX11005: Creating a SecurityKeyIdentifierClause is not supported.";
        public const string IDX11006 = "IDX11006: This method is not supported to read a 'saml2token' use the method: ReadToken(string securityToken, TokenValidationParameters validationParameters).";
        public const string IDX11007 = "IDX11007: This method is not supported to read a 'samltoken' use the method: ReadToken(string securityToken, TokenValidationParameters validationParameters).";
        public const string IDX11008 = "IDX11008: This method is not supported to validate a 'jwt' use the method: ValidateToken(String, TokenValidationParameters, out SecurityToken).";

        // Loading from web.config
        public const string IDX13000 = "IDX13000: A NamedKey must specify the 'symmetricKey' attribute. XML received: '{0}'.";
        public const string IDX13001 = "IDX13001: A NamedKey must specify the 'name' attribute. XML received: '{0}'.";
        public const string IDX13002 = "IDX13002: Attribute: '{0}' is null or whitespace.\nelement.OuterXml: '{1}'.";
        public const string IDX13003 = "IDX13003: EncodingType attribute must be one of: '{0}', '{1}', '{2}'. Encodingtype found: '{3}' XML : '{4}'.";

        // utility errors
        public const string IDX14700 = "IDX14700: Unable to decode: '{0}' as Base64url encoded string.";

        #pragma warning restore 1591


    }
}