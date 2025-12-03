using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using CartProductCommand = Ambev.DeveloperEvaluation.Application.Carts.CreateCart.CartProductCommand;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateCartHandler"/> class.
/// </summary>
public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly CreateCartHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCartHandlerTests"/> class.
    /// Sets up the test dependencies using NSubstitute.
    /// </summary>
    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCartHandler(_cartRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid cart creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid cart data When creating cart Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = CartTestData.GenerateValidCart();
        cart.UserId = command.UserId;
        cart.Date = command.Date;
        var result = new CreateCartResult
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Date = cart.Date
        };

        _mapper.Map<CreateCartResult>(cart).Returns(result);
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        var createCartResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createCartResult.Should().NotBeNull();
        createCartResult.Id.Should().Be(cart.Id);
        createCartResult.UserId.Should().Be(cart.UserId);
        await _cartRepository.Received(1).CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid cart creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid cart data When creating cart Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateCartCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that cart items are created from command products.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates cart items from products")]
    public async Task Handle_ValidRequest_CreatesCartItemsFromProducts()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = CartTestData.GenerateValidCart();
        cart.UserId = command.UserId;
        cart.Date = command.Date;
        var result = new CreateCartResult
        {
            Id = cart.Id
        };

        _mapper.Map<CreateCartResult>(cart).Returns(result);
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _cartRepository.Received(1).CreateAsync(
            Arg.Is<Cart>(c =>
                c.UserId == command.UserId &&
                c.Date == command.Date &&
                c.Products.Count == command.Products.Count &&
                c.Products.All(p => command.Products.Any(cp => cp.ProductId == p.ProductId && cp.Quantity == p.Quantity))),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the repository is called with the correct cart.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then saves cart to repository")]
    public async Task Handle_ValidRequest_SavesCartToRepository()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = CartTestData.GenerateValidCart();
        cart.UserId = command.UserId;
        cart.Date = command.Date;
        var result = new CreateCartResult
        {
            Id = cart.Id
        };

        _mapper.Map<CreateCartResult>(cart).Returns(result);
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _cartRepository.Received(1).CreateAsync(
            Arg.Is<Cart>(c =>
                c.UserId == command.UserId &&
                c.Date == command.Date),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the result is mapped from the created cart.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps created cart to result")]
    public async Task Handle_ValidRequest_MapsCreatedCartToResult()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = CartTestData.GenerateValidCart();
        cart.UserId = command.UserId;
        cart.Date = command.Date;
        var result = new CreateCartResult
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Date = cart.Date
        };

        _mapper.Map<CreateCartResult>(cart).Returns(result);
        _cartRepository.CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        var createCartResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CreateCartResult>(Arg.Is<Cart>(c => c.Id == cart.Id));
        createCartResult.Should().Be(result);
    }

    /// <summary>
    /// Tests that cart with empty products list throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given command with empty products When handling Then throws validation exception")]
    public async Task Handle_CommandWithEmptyProducts_ThrowsValidationException()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        command.Products = new List<CartProductCommand>();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}

