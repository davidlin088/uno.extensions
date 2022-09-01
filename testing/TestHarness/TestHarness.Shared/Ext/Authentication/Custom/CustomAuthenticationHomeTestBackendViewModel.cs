﻿namespace TestHarness.Ext.Authentication.Custom;

[ReactiveBindable(false)]
public partial class CustomAuthenticationHomeTestBackendViewModel : ObservableObject
{
	
	public INavigator Navigator { get; init; }
	public IAuthenticationService Authentication { get; init; }

	public ICustomAuthenticationTestBackendEndpoint Endpoint { get; init; }

	[ObservableProperty]
	private string[]? items;

	[ObservableProperty]
	private string? retrieveProductsResult;
	private ITokenCache Tokens { get; }


	public CustomAuthenticationHomeTestBackendViewModel(
		INavigator navigator,
		IAuthenticationService auth,
		ICustomAuthenticationTestBackendEndpoint endpoint,
		ITokenCache tokens)
	{
		Navigator = navigator;
		Authentication = auth;
		Endpoint = endpoint;
		Tokens = tokens;
	}

	public async void Logout()
	{
		await Authentication.LogoutAsync(CancellationToken.None);
		await Navigator.NavigateViewModelAsync<CustomAuthenticationLoginViewModel>(this, qualifier: Qualifiers.ClearBackStack);
	}

	public async void ClearAccessToken()
	{
		var creds = await Tokens.GetAsync();
		creds.Remove(TokenCacheExtensions.AccessTokenKey);
		await Tokens.SaveAsync(Tokens.CurrentProvider ?? string.Empty, creds);
	}

	public async void ClearAllTokens()
	{
		var creds = await Tokens.GetAsync();
		creds.Remove(TokenCacheExtensions.AccessTokenKey);
		creds.Remove(TokenCacheExtensions.RefreshTokenKey);
		await Tokens.SaveAsync(Tokens.CurrentProvider ?? string.Empty, creds);
	}

	public async void InvalidateTokens()
	{
		var creds = await Tokens.GetAsync();
		creds[TokenCacheExtensions.AccessTokenKey]= $"Some invalid access token {DateTime.Now.Ticks}";
		creds[TokenCacheExtensions.RefreshTokenKey] = $"Some invalid refresh token {DateTime.Now.Ticks}"; ;
		await Tokens.SaveAsync(Tokens.CurrentProvider ?? string.Empty, creds);
	}
	

	public async void Retrieve()
	{
		try
		{
			var response = await Endpoint.GetDataAuthorizationHeader(CancellationToken.None);
			Items = response?.ToArray();
			RetrieveProductsResult = Constants.CommerceProducts.ProductsLoadSuccess;
		}
		catch
		{
			RetrieveProductsResult = Constants.CommerceProducts.ProductsLoadError;
		}
	}
	public async void RetrieveCookie()
	{
		try
		{
			var response = await Endpoint.GetDataCookie(CancellationToken.None);
			Items = response?.ToArray();
			RetrieveProductsResult = Constants.CommerceProducts.ProductsLoadSuccess;
		}
		catch
		{
			RetrieveProductsResult = Constants.CommerceProducts.ProductsLoadError;
		}
	}
}
