//-----------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Tests;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// Tests for OpenIdConnectProtocolValidator
    /// </summary>
    public class OpenIdConnectProtocolValidatorTests
    {
        SampleListener eventListenerWarningLevel = new SampleListener();
        SampleListener eventListenerInfoLevel = new SampleListener();

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: GenerateNonce")]
        public void GenerateNonce()
        {
            List<string> errors = new List<string>();
            OpenIdConnectProtocolValidator protocolValidator = new OpenIdConnectProtocolValidator();
            string nonce = protocolValidator.GenerateNonce();
            int endOfTimestamp = nonce.IndexOf('.');
            if (endOfTimestamp == -1)
            {
                errors.Add("nonce does not have '.' seperator");
            }
            else
            {

            }
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: GetSets, test covers defaults")]
        public void GetSets()
        {
            OpenIdConnectProtocolValidator validationParameters = new OpenIdConnectProtocolValidator();
            Type type = typeof(OpenIdConnectProtocolValidator);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 9)
                Assert.True(true, "Number of properties has changed from 9 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("NonceLifetime", new List<object>{TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(100)}),
                        new KeyValuePair<string, List<object>>("RequireAcr", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAmr", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAuthTime", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireAzp", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireNonce", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireSub", new List<object>{false, true, false}),
                        new KeyValuePair<string, List<object>>("RequireTimeStampInNonce", new List<object>{true, false, true}),
                        new KeyValuePair<string, List<object>>("RequireStateValidation", new List<object>{true, false, true}),
                    },
                    Object = validationParameters,
                };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("OpenIdConnectProtocolValidator_GetSets", context.Errors);

            ExpectedException ee = ExpectedException.ArgumentNullException();
            Assert.NotNull(validationParameters.HashAlgorithmMap);
            Assert.Equal(validationParameters.HashAlgorithmMap.Count, 9);

            ee = ExpectedException.ArgumentOutOfRangeException();
            try
            {
                validationParameters.NonceLifetime = TimeSpan.Zero;
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        private void ValidateAuthenticationResponse(OpenIdConnectProtocolValidationContext context, OpenIdConnectProtocolValidator validator, ExpectedException ee)
        {
            try
            {
                validator.ValidateAuthenticationResponse(context);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        private void ValidateTokenResponse(OpenIdConnectProtocolValidationContext context, OpenIdConnectProtocolValidator validator, ExpectedException ee)
        {
            try
            {
                validator.ValidateTokenResponse(context);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: ValidateOpenIdConnectMessageWithIdTokenOnly")]
        public void ValidateMessageWithIdToken()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validNonce = Guid.NewGuid().ToString();
            var jwt = CreateValidatedIdToken();
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Nonce, validNonce));

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ValidatedIdToken = jwt,
                ProtocolMessage = new OpenIdConnectMessage
                {
                    IdToken = Guid.NewGuid().ToString(),
                    State = validState,
                },
                Nonce = validNonce,
                State = validState
            };

            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // no 'token' in the message
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithIdTokenCode")]
        public void ValidateMessageWithIdTokenCode()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validNonce = Guid.NewGuid().ToString();
            var validCode = Guid.NewGuid().ToString();
            var cHashClaim = IdentityUtilities.CreateHashClaim(validCode, "SHA256");
            var jwt = CreateValidatedIdToken();
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Nonce, validNonce));
            jwt.Header[JwtHeaderParameterNames.Alg] = "SHA256";

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ValidatedIdToken = jwt,
                ProtocolMessage = new OpenIdConnectMessage
                {
                    IdToken = Guid.NewGuid().ToString(),
                    State = validState,
                    Code = validCode
                },
                Nonce = validNonce,
                State = validState
            };

            // code present, but no chash claim
            ValidateAuthenticationResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10307:")
                );
            // no 'token' in the message
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );

            // adding chash claim
            protocolValidationContext.ValidatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.CHash, cHashClaim));
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);
            // no 'token' in the message
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithIdTokenCodeToken")]
        public void ValidateMessageWithIdTokenCodeToken()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validNonce = Guid.NewGuid().ToString();
            var validCode = Guid.NewGuid().ToString();
            var validAccessToken = Guid.NewGuid().ToString();
            var cHashClaim = IdentityUtilities.CreateHashClaim(validCode, "SHA256");
            var atHashClaim = IdentityUtilities.CreateHashClaim(validAccessToken, "SHA256");
            var jwt = CreateValidatedIdToken();
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Nonce, validNonce));
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.CHash, cHashClaim));
            jwt.Header[JwtHeaderParameterNames.Alg] = "SHA256";

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ValidatedIdToken = jwt,
                ProtocolMessage = new OpenIdConnectMessage
                {
                    IdToken = Guid.NewGuid().ToString(),
                    State = validState,
                    Code = validCode,
                    Token = validAccessToken
                },
                Nonce = validNonce,
                State = validState
            };

            // token present, but no atHash claim
            ValidateAuthenticationResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX10312:")
                );
            // no exception since 'at_hash' claim is optional
            ValidateTokenResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // adding atHash claim
            protocolValidationContext.ValidatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.AtHash, atHashClaim));
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);
            ValidateTokenResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithIdTokenToken")]
        public void ValidateMessageWithIdTokenToken()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validNonce = Guid.NewGuid().ToString();
            var validAccessToken = Guid.NewGuid().ToString();
            var atHashClaim = IdentityUtilities.CreateHashClaim(validAccessToken, "SHA256");
            var jwt = CreateValidatedIdToken();
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Nonce, validNonce));
            jwt.Header[JwtHeaderParameterNames.Alg] = "SHA256";

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ValidatedIdToken = jwt,
                ProtocolMessage = new OpenIdConnectMessage
                {
                    IdToken = Guid.NewGuid().ToString(),
                    State = validState,
                    Token = validAccessToken
                },
                Nonce = validNonce,
                State = validState
            };

            // token present, but no atHash claim
            ValidateAuthenticationResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX10312:")
                );
            // no exception since 'at_hash' claim is optional
            ValidateTokenResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // adding atHash claim
            protocolValidationContext.ValidatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.AtHash, atHashClaim));
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);
            ValidateTokenResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithCode")]
        public void ValidateMessageWithCode()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireNonce = false };
            var validState = Guid.NewGuid().ToString();

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = new OpenIdConnectMessage
                {
                    State = validState,
                    Code = Guid.NewGuid().ToString()
                }
            };

            // 'RequireStateValidation' is true but no state passed in validationContext
            ValidateAuthenticationResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX10329:")
                );

            // turn off state validation
            protocolValidator.RequireStateValidation = false;
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // turn on state validation and add valid state
            protocolValidator.RequireStateValidation = true;
            protocolValidationContext.State = validState;
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // absence of 'id_token' and 'token'
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithToken")]
        public void ValidateMessageWithToken()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validAccessToken = Guid.NewGuid().ToString();

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = new OpenIdConnectMessage
                {
                    State = validState,
                    Token = validAccessToken
                },
                State = validState
            };

            // token present, but no 'id_token'
            ValidateAuthenticationResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10334:")
                );
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidator: ValidateMessageWithCodeToken")]
        public void ValidateMessageWithCodeToken()
        {
            var protocolValidator = new OpenIdConnectProtocolValidator { RequireTimeStampInNonce = false };
            var validState = Guid.NewGuid().ToString();
            var validCode = Guid.NewGuid().ToString();
            var validAccessToken = Guid.NewGuid().ToString();
            var cHashClaim = IdentityUtilities.CreateHashClaim(validCode, "SHA256");
            var atHashClaim = IdentityUtilities.CreateHashClaim(validAccessToken, "SHA256");

            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = new OpenIdConnectMessage
                {
                    State = validState,
                    Code = validCode,
                    Token = validAccessToken
                },
                State = validState
            };

            // code present, but no 'id_token'
            ValidateAuthenticationResponse(protocolValidationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // 'code' and 'token' present but no 'id_token'
            ValidateTokenResponse(
                protocolValidationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10336:")
                );
        }

        private JwtSecurityToken CreateValidatedIdToken()
        {
            var jwt = new JwtSecurityToken();
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, IdentityUtilities.DefaultAudience));
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.UtcNow).ToString()));
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()));
            jwt.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, IdentityUtilities.DefaultIssuer));
            return jwt;
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: ValidateAuthenticationResponse")]
        public void ValidateAuthenticationResponse()
        {
            var validator = new PublicOpenIdConnectProtocolValidator { RequireStateValidation = false };
            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = new OpenIdConnectMessage()
            };

            // validationContext is null
            ValidateAuthenticationResponse(null, validator, ExpectedException.ArgumentNullException());

            // validationContext.ProtocolMessage is null
            ValidateAuthenticationResponse(
                new OpenIdConnectProtocolValidationContext(),
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10333:")
                );

            // validationContext.ProtocolMessage.IdToken is null
            ValidateAuthenticationResponse(
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10334:")
                );

            // validationContext.ProtocolMessage.IdToken is not null, whereas validationContext.validatedIdToken is null
            protocolValidationContext.ProtocolMessage.IdToken = Guid.NewGuid().ToString();
            ValidateAuthenticationResponse(
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10331:")
                );

            // 'refresh_token' should not be present in the response received from Authorization Endpoint
            protocolValidationContext.ValidatedIdToken = new JwtSecurityToken();
            protocolValidationContext.ProtocolMessage.RefreshToken = "refresh_token";
            ValidateAuthenticationResponse(
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10335:")
                );
        }

        private void ValidateIdToken(JwtSecurityToken jwt, OpenIdConnectProtocolValidationContext validationContext, PublicOpenIdConnectProtocolValidator protocolValidator, ExpectedException ee)
        {
            try
            {
                protocolValidator.PublicValidateIdToken(jwt, validationContext);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }

            return;
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: Validation of IdToken")]
        public void ValidateIdToken()
        {
            var validator = new PublicOpenIdConnectProtocolValidator { RequireStateValidation = false };
            var protocolValidationContext = new OpenIdConnectProtocolValidationContext
            {
                ProtocolMessage = new OpenIdConnectMessage()
            };
            var validatedIdToken = new JwtSecurityToken();
            eventListenerWarningLevel.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            // aud missing
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10314:")
                );

            // exp missing
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, IdentityUtilities.DefaultAudience));
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10314:")
                );

            // iat missing
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.UtcNow).ToString()));
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10314:")
                );

            // iss missing
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()));
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10314:")
                );

            // add iss, nonce is not required, state not required
            validator.RequireNonce = false;
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, IdentityUtilities.DefaultIssuer));
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);

            // missing 'sub'
            validator.RequireSub = true;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10314:")
                );
            validator.RequireSub = false;

            // validate optional claims, 'acr' claim
            validator.RequireAcr = true;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10315:")
                );
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Acr, "acr"));

            // 'amr' claim
            validator.RequireAmr = true;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10316:")
                );
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Amr, "amr"));

            // 'auth_time' claim
            validator.RequireAuthTime = true;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10317:")
                );
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.AuthTime, "authTime"));

            // multiple 'aud' but no 'azp' claim. no exception thrown, warning logged
            validatedIdToken.Payload[JwtRegisteredClaimNames.Aud] = new List<string> { "abc", "xyz"};
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);
            Assert.Contains("IDX10339: ", eventListenerWarningLevel.TraceBuffer);

            // 'azp' claim
            validator.RequireAzp = true;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10318:")
                );
            validatedIdToken.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Azp, "azp"));

            // 'azp' claim present but 'client_id' is null
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);
            Assert.Contains("IDX10308: ", eventListenerWarningLevel.TraceBuffer);

            // 'azp' claim present but 'client_id' does not match
            protocolValidationContext.ClientId = "client_id";
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);
            Assert.Contains("IDX10340: ", eventListenerWarningLevel.TraceBuffer);

            // all claims present, no exception expected
            protocolValidationContext.ClientId = "azp";
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);

            // validating the delegate
            IdTokenValidator idTokenValidatorThrows = ((jwt, context) => { throw new OpenIdConnectProtocolException(); });
            IdTokenValidator idTokenValidatorReturns = ((jwt, context) => { return; });
            IdTokenValidator idTokenValidatorValidateAcr =
                ((jwt, context) =>
                {
                    JwtSecurityToken jwtSecurityToken = jwt as JwtSecurityToken;
                    if (jwtSecurityToken.Payload.Acr != "acr")
                        throw new OpenIdConnectProtocolException();
                });
            validator.IdTokenValidator = idTokenValidatorThrows;
            ValidateIdToken(
                validatedIdToken,
                protocolValidationContext,
                validator,
                new ExpectedException(typeof(OpenIdConnectProtocolException))
                );

            validator.IdTokenValidator = idTokenValidatorReturns;
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);

            validator.IdTokenValidator = idTokenValidatorValidateAcr;
            ValidateIdToken(validatedIdToken, protocolValidationContext, validator, ExpectedException.NoExceptionExpected);
        }

        private void ValidateCHash(JwtSecurityToken jwt, OpenIdConnectProtocolValidationContext validationContext, PublicOpenIdConnectProtocolValidator protocolValidator, ExpectedException ee)
        {
            try
            {
                protocolValidator.PublicValidateCHash(jwt, validationContext);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }

            return;
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: Validation of CHash")]
        public void Validate_CHash()
        {
            var protocolValidator = new PublicOpenIdConnectProtocolValidator();

            string authorizationCode1 = protocolValidator.GenerateNonce();
            string authorizationCode2 = protocolValidator.GenerateNonce();

            string chash1 = IdentityUtilities.CreateHashClaim(authorizationCode1, "SHA256");
            string chash2 = IdentityUtilities.CreateHashClaim(authorizationCode2, "SHA256");

            Dictionary<string, string> emptyDictionary = new Dictionary<string, string>();
            Dictionary<string, string> mappedDictionary = new Dictionary<string, string>(protocolValidator.HashAlgorithmMap);

            JwtSecurityToken jwtWithCHash1 =
                new JwtSecurityToken
                (
                    audience: IdentityUtilities.DefaultAudience,
                    claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.CHash, chash1) },
                    issuer: IdentityUtilities.DefaultIssuer
                );

            JwtSecurityToken jwtWithEmptyCHash =
                new JwtSecurityToken
                (
                    audience: IdentityUtilities.DefaultAudience,
                    claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.CHash, string.Empty) },
                    issuer: IdentityUtilities.DefaultIssuer,
                    signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                );

            JwtSecurityToken jwtWithoutCHash =
                new JwtSecurityToken
                (
                    audience: IdentityUtilities.DefaultAudience,
                    issuer: IdentityUtilities.DefaultIssuer                    
                );

            JwtSecurityToken jwtWithSignatureChash1 = 
                new JwtSecurityToken
                (
                    audience : IdentityUtilities.DefaultAudience,
                    claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.CHash, chash1) },
                    issuer: IdentityUtilities.DefaultIssuer,
                    signingCredentials : IdentityUtilities.DefaultAsymmetricSigningCredentials
                );

            JwtSecurityToken jwtWithSignatureMultipleChashes =
                new JwtSecurityToken
                (
                    audience: IdentityUtilities.DefaultAudience,
                    claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.CHash, chash1), new Claim(JwtRegisteredClaimNames.CHash, chash2) },
                    issuer: IdentityUtilities.DefaultIssuer,
                    signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                );


            OpenIdConnectProtocolValidationContext validationContext = new OpenIdConnectProtocolValidationContext();
            validationContext.ProtocolMessage = new OpenIdConnectMessage
            {
                Code = authorizationCode2
            };

            // chash is not a string, but array
            ValidateCHash(
                jwtWithSignatureMultipleChashes,
                validationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10306:")
                );

            // chash doesn't match
            ValidateCHash(
                jwtWithSignatureChash1,
                validationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10300:", typeof(OpenIdConnectProtocolException))
                );

            // valid code
            validationContext.ProtocolMessage = new OpenIdConnectMessage
            {
                Code = authorizationCode1
            };

            ValidateCHash(jwtWithSignatureChash1, validationContext, protocolValidator, ExpectedException.NoExceptionExpected);

            // 'id_token' is null
            ValidateCHash(null, validationContext, protocolValidator, ExpectedException.ArgumentNullException());
            // validationContext is null
            ValidateCHash(jwtWithoutCHash, null, protocolValidator, ExpectedException.ArgumentNullException());

            // 'c_hash' claim is not present
            ValidateCHash(
                jwtWithoutCHash,
                validationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10307:")
                );
            // empty 'c_hash' claim
            ValidateCHash(
                jwtWithEmptyCHash,
                validationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10300:", typeof(OpenIdConnectProtocolException))
                );
            // algorithm mismatch. header.alg is 'None'.
            ValidateCHash(
                jwtWithCHash1,
                validationContext,
                protocolValidator,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidCHashException), "IDX10302:", typeof(OpenIdConnectProtocolException))
                );

            // make sure default alg works.
            validationContext.ProtocolMessage.Code = authorizationCode1;
            jwtWithCHash1.Header.Remove("alg");
            ValidateCHash(jwtWithCHash1, validationContext, protocolValidator, ExpectedException.NoExceptionExpected);
        }

        private void ValidateNonce(JwtSecurityToken jwt, PublicOpenIdConnectProtocolValidator protocolValidator, OpenIdConnectProtocolValidationContext validationContext, ExpectedException ee)
        {
            try
            {
                protocolValidator.PublicValidateNonce(jwt, validationContext);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        [Fact(DisplayName = "OpenIdConnectProtocolValidatorTests: Validation of Nonce")]
        public void Validate_Nonce()
        {
            PublicOpenIdConnectProtocolValidator protocolValidatorRequiresTimeStamp = new PublicOpenIdConnectProtocolValidator();
            string nonceWithTimeStamp = protocolValidatorRequiresTimeStamp.GenerateNonce();

            PublicOpenIdConnectProtocolValidator protocolValidatorDoesNotRequireTimeStamp =
                new PublicOpenIdConnectProtocolValidator
                {
                    RequireTimeStampInNonce = false,
                };

            PublicOpenIdConnectProtocolValidator protocolValidatorDoesNotRequireNonce =
               new PublicOpenIdConnectProtocolValidator
               {
                   RequireNonce = false,
               };

            string nonceWithoutTimeStamp = protocolValidatorDoesNotRequireTimeStamp.GenerateNonce();
            string nonceBadTimeStamp = "abc.abc";
            string nonceTicksTooLarge = Int64.MaxValue.ToString() + "." + nonceWithoutTimeStamp;
            string nonceTicksTooSmall = Int64.MinValue.ToString() + "." + nonceWithoutTimeStamp;
            string nonceTicksNegative = ((Int64)(-1)).ToString() + "." + nonceWithoutTimeStamp;
            string nonceTicksZero = ((Int64)(0)).ToString() + "." + nonceWithoutTimeStamp;

            JwtSecurityToken jwtWithNonceWithTimeStamp = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceWithTimeStamp) });
            JwtSecurityToken jwtWithNonceWithoutTimeStamp = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceWithoutTimeStamp) });
            JwtSecurityToken jwtWithNonceWithBadTimeStamp = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceBadTimeStamp) });
            JwtSecurityToken jwtWithNonceTicksTooLarge = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceTicksTooLarge) });
            JwtSecurityToken jwtWithNonceTicksTooSmall = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceTicksTooSmall) });
            JwtSecurityToken jwtWithNonceTicksNegative = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceTicksNegative) });
            JwtSecurityToken jwtWithNonceZero = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, nonceTicksZero) });
            JwtSecurityToken jwtWithoutNonce = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, nonceWithTimeStamp) });
            JwtSecurityToken jwtWithNonceWhitespace = new JwtSecurityToken(claims: new List<Claim> { new Claim(JwtRegisteredClaimNames.Nonce, "") });

            OpenIdConnectProtocolValidationContext validationContext = new OpenIdConnectProtocolValidationContext();

            validationContext.Nonce = null;
            // id_token is null
            ValidateNonce( null, protocolValidatorRequiresTimeStamp, validationContext, ExpectedException.ArgumentNullException());
            // validationContext is null
            ValidateNonce(jwtWithNonceWithTimeStamp, protocolValidatorRequiresTimeStamp, null, ExpectedException.ArgumentNullException());
            // validationContext.nonce is null, RequireNonce is true.
            ValidateNonce(
                jwtWithNonceWithTimeStamp,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10320:")
                );

            validationContext.Nonce = nonceWithoutTimeStamp;
            // idToken.nonce is null, validationContext.nonce is not null
            ValidateNonce(
                jwtWithoutNonce,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10323:")
                );
            // nonce does not match
            ValidateNonce(
                jwtWithNonceWhitespace,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10321:")
                );
            ValidateNonce(
                jwtWithNonceWithTimeStamp,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10321:")
                );

            // nonce match
            validationContext.Nonce = nonceWithTimeStamp;
            ValidateNonce(jwtWithNonceWithTimeStamp, protocolValidatorRequiresTimeStamp, validationContext, ExpectedException.NoExceptionExpected);

            // nonce expired
            validationContext.Nonce = nonceWithTimeStamp;
            protocolValidatorRequiresTimeStamp.NonceLifetime = TimeSpan.FromMilliseconds(10);
            Thread.Sleep(100);
            ValidateNonce(
                jwtWithNonceWithTimeStamp,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10324: ")
                );

            // nonce missing timestamp, validator requires time stamp
            // 1. no time stamp
            validationContext.Nonce = nonceWithoutTimeStamp;
            protocolValidatorRequiresTimeStamp.NonceLifetime = TimeSpan.FromMinutes(10);
            ValidateNonce(
                jwtWithNonceWithoutTimeStamp,
                protocolValidatorRequiresTimeStamp, 
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10325:")
                );

            // 2. timestamp not well formed
            validationContext.Nonce = nonceBadTimeStamp;
            ValidateNonce(
                jwtWithNonceWithBadTimeStamp,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10326:", typeof(FormatException))
                );

            // 3. timestamp not required
            validationContext.Nonce = nonceBadTimeStamp;
            ValidateNonce(jwtWithNonceWithBadTimeStamp, protocolValidatorDoesNotRequireTimeStamp, validationContext, ExpectedException.NoExceptionExpected);

            // 4. ticks max value
            validationContext.Nonce = nonceTicksTooLarge;
            ValidateNonce(
                jwtWithNonceTicksTooLarge,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10327:", typeof(ArgumentException))
                );

            // 5. ticks min value small
            validationContext.Nonce = nonceTicksTooSmall;
            ValidateNonce(
                jwtWithNonceTicksTooSmall,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10326:")
                );

            // 6. ticks negative
            validationContext.Nonce = nonceTicksNegative;
            ValidateNonce(
                jwtWithNonceTicksNegative,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10326:")
                );

            // 7. ticks zero
            validationContext.Nonce = nonceTicksZero;
            ValidateNonce(
                jwtWithNonceZero,
                protocolValidatorRequiresTimeStamp,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10326:")
                );

            // require nonce false
            validationContext.Nonce = null;
            ValidateNonce(jwtWithNonceWithoutTimeStamp, protocolValidatorDoesNotRequireNonce, validationContext, ExpectedException.NoExceptionExpected);

            // validationContext has nonce, idToken.nonce is null and requireNonce is false
            validationContext.Nonce = nonceWithTimeStamp;
            ValidateNonce(
                jwtWithoutNonce,
                protocolValidatorDoesNotRequireNonce,
                validationContext,
                new ExpectedException(typeof(OpenIdConnectProtocolInvalidNonceException), "IDX10323:")
                );
            // idToken.Nonce is not null
            ValidateNonce(jwtWithNonceWithTimeStamp, protocolValidatorDoesNotRequireNonce, validationContext, ExpectedException.NoExceptionExpected);

        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [Theory, MemberData("AtHashDataSet")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public void Validate_AtHash(JwtSecurityToken jwt, OpenIdConnectProtocolValidationContext context, PublicOpenIdConnectProtocolValidator validator, ExpectedException ee)
        {
            try
            {
                validator.PublicValidateAtHash(jwt, context);
                ee.ProcessNoException();
            }
            catch(Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<JwtSecurityToken, OpenIdConnectProtocolValidationContext, PublicOpenIdConnectProtocolValidator, ExpectedException> AtHashDataSet
        {
            get
            {
                var dataset = new TheoryData<JwtSecurityToken, OpenIdConnectProtocolValidationContext, PublicOpenIdConnectProtocolValidator, ExpectedException>();
                var validator = new PublicOpenIdConnectProtocolValidator();
                var token = Guid.NewGuid().ToString();
                var hashClaimValue256 = IdentityUtilities.CreateHashClaim(token, "SHA256");
                var hashClaimValue512 = IdentityUtilities.CreateHashClaim(token, "SHA512");

                dataset.Add(
                    null,
                    new OpenIdConnectProtocolValidationContext(),
                    validator,
                    new ExpectedException(typeof(ArgumentNullException))
                );
                dataset.Add(
                    new JwtSecurityToken(),
                    new OpenIdConnectProtocolValidationContext(),
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolException), "IDX10333:")
                );
                dataset.Add(
                    null,
                    new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            IdToken = Guid.NewGuid().ToString(),
                            Token = token
                        }
                    },
                    validator,
                    new ExpectedException(typeof(ArgumentNullException))
                );
                dataset.Add(
                    new JwtSecurityToken(
                        claims: new List<Claim> { new Claim("at_hash", hashClaimValue256) },
                        signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                        ),
                    new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            Token = token,
                        }
                    },
                    validator,
                    ExpectedException.NoExceptionExpected
                );
                dataset.Add(
                    new JwtSecurityToken
                        (
                            claims: new List<Claim> { new Claim("at_hash", hashClaimValue512) },
                            signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                        ),
                    new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            Token = token,
                        }
                    },
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX10300:", typeof(OpenIdConnectProtocolException))
                );
                dataset.Add(
                    new JwtSecurityToken
                        (
                            claims: new List<Claim> { new Claim("at_hash", hashClaimValue256) },
                            signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                        ),
                    new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            Token = Guid.NewGuid().ToString(),
                        }
                    },
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX10300:", typeof(OpenIdConnectProtocolException))
                );
                dataset.Add(
                    new JwtSecurityToken
                        (
                            claims: new List<Claim> { new Claim("at_hash", hashClaimValue256), new Claim("at_hash", hashClaimValue256) },
                            signingCredentials: IdentityUtilities.DefaultAsymmetricSigningCredentials
                        ),
                    new OpenIdConnectProtocolValidationContext()
                    {
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            Token = Guid.NewGuid().ToString(),
                        }
                    },
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidAtHashException), "IDX10311:")
                );

                return dataset;
            }
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [Theory, MemberData("StateDataSet")]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        public void Validate_State(OpenIdConnectProtocolValidationContext context, PublicOpenIdConnectProtocolValidator validator, ExpectedException ee)
        {
            try
            {
                validator.PublicValidateState(context);
                ee.ProcessNoException();
            }
            catch (Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        public static TheoryData<OpenIdConnectProtocolValidationContext, PublicOpenIdConnectProtocolValidator, ExpectedException> StateDataSet
        {
            get
            {
                var dataset = new TheoryData<OpenIdConnectProtocolValidationContext, PublicOpenIdConnectProtocolValidator, ExpectedException>();
                var validator = new PublicOpenIdConnectProtocolValidator();
                var validatorRequireStateFalse = new PublicOpenIdConnectProtocolValidator { RequireStateValidation = false };
                var state1 = Guid.NewGuid().ToString();
                var state2 = Guid.NewGuid().ToString();

                dataset.Add(null, validator, ExpectedException.ArgumentNullException());
                dataset.Add(
                    new OpenIdConnectProtocolValidationContext(),
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX10329:")
                );
                dataset.Add(
                    new OpenIdConnectProtocolValidationContext(),
                    validatorRequireStateFalse,
                    ExpectedException.NoExceptionExpected
                );
                dataset.Add(
                    new OpenIdConnectProtocolValidationContext
                    {
                        State = state1,
                    },
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX10330:")
                );
                dataset.Add(
                    new OpenIdConnectProtocolValidationContext()
                    {
                        State = state1,
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            State = state1
                        }
                    },
                    validator,
                    ExpectedException.NoExceptionExpected
                );
                dataset.Add(
                    new OpenIdConnectProtocolValidationContext()
                    {
                        State = state1,
                        ProtocolMessage = new OpenIdConnectMessage
                        {
                            State = state2
                        }
                    },
                    validator,
                    new ExpectedException(typeof(OpenIdConnectProtocolInvalidStateException), "IDX10331:")
                );
                return dataset;
            }
        }
    }

    public class PublicOpenIdConnectProtocolValidator : OpenIdConnectProtocolValidator
    {
        public void PublicValidateIdToken(JwtSecurityToken token, OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateIdToken(token, context);
        }

        public void PublicValidateCHash(JwtSecurityToken token, OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateCHash(token, context);
        }

        public void PublicValidateAtHash(JwtSecurityToken token, OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateAtHash(token, context);
        }

        public void PublicValidateNonce(JwtSecurityToken token, OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateNonce(token, context);
        }

        public void PublicValidateState(OpenIdConnectProtocolValidationContext context)
        {
            base.ValidateState(context);
        }

        public void SetHashAlgorithmMap(Dictionary<string, string> hashAlgorithmMap)
        {
            HashAlgorithmMap.Clear();
            foreach (var key in hashAlgorithmMap.Keys)
                HashAlgorithmMap.Add(key, hashAlgorithmMap[key]);
        }
    }

    class SampleListener : EventListener
    {
        public string TraceBuffer { get; set; }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData != null && eventData.Payload.Count > 0)
            {
                TraceBuffer += eventData.Payload[0] + "\n";
            }
        }
    }
}
