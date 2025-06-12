package com.app.sanimex.core.navigation

import androidx.compose.animation.core.SpringSpec
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.navigation.NavHostController
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.navArgument
import com.app.sanimex.core.util.Constants
import com.app.sanimex.presentation.screens.ClinteMayoreo.ClienteScreen
import com.app.sanimex.presentation.screens.HisCotizacion.HisCotizacionScreen
import com.app.sanimex.presentation.screens.HisCotizacionG.HisCotizacionGScreen
import com.app.sanimex.presentation.screens.HisCotizacionGDetalle.HistCotizacionGDScreen
import com.app.sanimex.presentation.screens.HisCtoDetalle.HisCtoDetalleScreen
import com.app.sanimex.presentation.screens.HisCtoDetalleGerente.HisCtoDetalleGerenteScreen
import com.app.sanimex.presentation.screens.account.AccountScreen
import com.app.sanimex.presentation.screens.auth.login.LoginScreen
import com.app.sanimex.presentation.screens.cart.CartScreen
import com.app.sanimex.presentation.screens.explore.ExploreScreen
import com.app.sanimex.presentation.screens.favorites.FavoritesScreen
import com.app.sanimex.presentation.screens.offer.OfferScreen
import com.app.sanimex.presentation.screens.search_results.SearchResultsScreen
import com.app.sanimex.presentation.screens.single_product.SingleProductScreen
import com.app.sanimex.presentation.screens.MapsSucursal.MapsSucursalScreen
import com.app.sanimex.presentation.screens.RutaVisitador.RutaVisitadorScreen
import com.app.sanimex.presentation.screens.VisitadorActivo.VisitadorActivoScreen
import com.app.sanimex.presentation.screens.tipoIngreso.TipoIngresoScreen

/**
 * Componente composable que define la navegación principal de la aplicación.
 *
 * Utiliza un [NavHost] para gestionar las diferentes pantallas y las transiciones entre ellas.
 *
 * @param navController El [NavHostController] que gestiona la navegación dentro del [NavHost].
 * @param startDestination La ruta de la pantalla de inicio que se mostrará al iniciar la navegación.
 * @author David Duarte
 * @version 1.0
 */
@Composable
fun AppNavigation(
    navController: NavHostController,
    startDestination: String,
) {
    /**
     * Estado para almacenar el ID del producto seleccionado.
     * Actualmente no se utiliza en la navegación definida, pero podría usarse para pasar información entre pantallas.
     */
    var selectedProductID by remember { mutableStateOf<String?>(null) }

    NavHost(
        navController = navController,
        startDestination = startDestination,
        /**
         * Definición de la animación de entrada para las transiciones entre pantallas.
         * Utiliza un [SpringSpec] para crear un efecto de rebote suave.
         */
        enterTransition = {
            fadeIn(animationSpec = SpringSpec(dampingRatio = 0.6573f, stiffness = 100f))
        },
        /**
         * Definición de la animación de salida para las transiciones entre pantallas.
         * Utiliza un [SpringSpec] para crear un efecto de desvanecimiento suave.
         */
        exitTransition = {
            fadeOut(animationSpec = SpringSpec(dampingRatio = 0.6573f, stiffness = 100f))
        }
    ) {
        /**
         * Grupo de rutas relacionadas con la autenticación.
         */
        composable(route = AppScreens.Login.route) {
            /**
             * Pantalla de inicio de sesión.
             *
             * Proporciona lambdas para navegar a la pantalla de registro, la pantalla principal (Favorites)
             * y la pantalla de recuperación de contraseña.
             */
            LoginScreen(
                onNavigateToHome = {
                    navController.navigate(route = AppScreens.Favorites.route) {
                        /**
                         * Al navegar a la pantalla principal, elimina la pantalla de inicio de sesión de la pila de navegación.
                         * `inclusive = true` asegura que la propia pantalla de inicio de sesión también se elimine.
                         */
                        this.popUpTo(AppScreens.Login.route) {
                            this.inclusive = true
                        }
                    }
                }
            )
        }

        composable(route = AppScreens.Cliente.route) {
            /**
             * Pantalla para seleccionar el tipo de cliente.
             *
             * Proporciona una lambda para navegar a la siguiente pantalla (TipoIngreso),
             * eliminando la pantalla actual de la pila de navegación.
             */
            ClienteScreen(
                onNavigateToHome = {
                    navController.navigate(route = AppScreens.TipoIngreso.route) {
                        /**
                         * Al navegar a la siguiente pantalla, elimina la pantalla actual de la pila de navegación.
                         * `inclusive = true` asegura que la propia pantalla de cliente también se elimine.
                         */
                        this.popUpTo(AppScreens.Cliente.route) {
                            this.inclusive = true
                        }
                    }
                },
            )
        }
        composable(route = AppScreens.TipoIngreso.route) {
            /**
             * Pantalla para seleccionar el tipo de ingreso.
             *
             * Proporciona una lambda para navegar a la pantalla principal de exploración (Explore),
             * eliminando la pantalla actual de la pila de navegación.
             */
            TipoIngresoScreen(
                onNavigateToHome = {
                    navController.navigate(route = AppScreens.Explore.route) {
                        /**
                         * Al navegar a la pantalla de exploración, elimina la pantalla actual de la pila de navegación.
                         * `inclusive = true` asegura que la propia pantalla de tipo de ingreso también se elimine.
                         */
                        popUpTo(route = AppScreens.TipoIngreso.route){
                            inclusive = true
                        }
                    }
                },
            )
        }

        composable(route = AppScreens.Explore.route) {
                 if(Constants.corredor.isEmpty()) {
                LaunchedEffect(Unit) {
                    navController.navigate(route = AppScreens.Favorites.route)
                }
            }
                ExploreScreen(
                    onNavigateToHome = {
                        Constants.corredor = ""
                        navController.navigate(route = AppScreens.Favorites.route) {
                            popUpTo(route = AppScreens.Explore.route){
                                inclusive = true
                            }
                        }
                        navController.navigateUp()
                    },
                    onNavigateToCart = {
                        navController.navigate(route = AppScreens.Cart.route) {
                            popUpTo(route = AppScreens.Cart.route)
                        }
                    },
                    onNavigateToUbicacion = {
                        navController.navigate(route = AppScreens.MapsSucursal.route) {
                            popUpTo(route = AppScreens.MapsSucursal.route)
                        }
                    },
                    onNavigateToTicket = {
                        navController.navigate(route = AppScreens.HistorialCotizacion.route) {
                            popUpTo(route = AppScreens.HistorialCotizacion.route)
                        }
                    },
                    onNavigateToTicketGerente = {
                        navController.navigate(route = AppScreens.HistorialCotizacionGerente.route) {
                            popUpTo(route = AppScreens.HistorialCotizacionGerente.route)
                        }
                    },
                    onNavigateToAccount = {
                        navController.navigate(route = AppScreens.Account.route) {
                            popUpTo(route = AppScreens.Account.route)
                        }
                    },

                    onNavigateToFav = {
                        navController.navigate(route = AppScreens.Cliente.route){
                            popUpTo(AppScreens.Favorites.route) {
                                inclusive = true
                            }
                        }
                    },
                    onNavigateToSearchResults = { productID ->
                        navController.navigate(
                            route = AppScreens.SingleProductInfo.route.replace(
                                "{product_id}",
                                productID
                            )
                        )
                    },
                )
        }

        composable(route = AppScreens.Cart.route) {
            CartScreen(
                onNavigationBack = {
                    navController.popBackStack()
                },
                onNavigateToExplore = {
                    navController.navigate(route = AppScreens.Explore.route) {
                        popUpTo(route = AppScreens.Explore.route)
                    }
                },
                onNavigateToUbicacion = {
                    navController.navigate(route = AppScreens.MapsSucursal.route) {
                        popUpTo(route = AppScreens.MapsSucursal.route)
                    }
                },
                onNavigateToTicket = {
                    navController.navigate(route = AppScreens.HistorialCotizacion.route) {
                        popUpTo(route = AppScreens.HistorialCotizacion.route)
                    }
                },
                onNavigateToTicketGerente = {
                    navController.navigate(route = AppScreens.HistorialCotizacionGerente.route) {
                        popUpTo(route = AppScreens.HistorialCotizacionGerente.route)
                    }
                },
                onNavigateToAccount = {
                    navController.navigate(route = AppScreens.Account.route) {
                        popUpTo(route = AppScreens.Account.route)
                    }
                }
            )
        }

        composable(route = AppScreens.Offer.route) {
            OfferScreen(
                onNavigationBack = {
                    navController.navigateUp()
                },
            )
        }

        composable(route = AppScreens.Account.route) {
            AccountScreen(
                onNavigateToExplore = {
                    navController.navigate(route = AppScreens.Explore.route) {
                        popUpTo(route = AppScreens.Explore.route)
                    }
                },
                onNavigateToCart = {
                    navController.navigate(route = AppScreens.Cart.route) {
                        popUpTo(route = AppScreens.Cart.route)
                    }
                },
                onNavigateToUbicacion = {
                    navController.navigate(route = AppScreens.MapsSucursal.route) {
                        popUpTo(route = AppScreens.MapsSucursal.route)
                    }
                },
                onNavigateToTicket = {
                    navController.navigate(route = AppScreens.HistorialCotizacion.route) {
                        popUpTo(route = AppScreens.HistorialCotizacion.route)
                    }
                },
                onNavigateToTicketGerente = {
                    navController.navigate(route = AppScreens.HistorialCotizacionGerente.route) {
                        popUpTo(route = AppScreens.HistorialCotizacionGerente.route)
                    }
                },

                onNavigateToLogin = {
                    navController.navigate(route = AppScreens.Login.route) {
                        popUpTo(route = AppScreens.Login.route) {
                            inclusive = true
                        }
                    }
                }
            )
        }


        composable(route = AppScreens.SingleProductInfo.route)
        {
            navBackStackEntry ->
            if(Constants.idDireccion == 0) {
                LaunchedEffect(Unit) {
                    navController.navigate(route = AppScreens.TipoIngreso.route)
                }
            }
            val productID = navBackStackEntry.arguments?.getCharSequence("product_id")
            if (productID != null) {
                SingleProductScreen(
                    productID = productID.toString(),
                    onBackClicked = {
                        navController.navigate(route = AppScreens.Explore.route) {
                            popUpTo(route = AppScreens.SingleProductInfo.route)
                        }
                    },
                    onNavigateToFav = {
                        navController.navigate(route = AppScreens.Favorites.route){
                            popUpTo(route = AppScreens.SingleProductInfo.route){
                                inclusive = true
                            }
                        }
                    },
                    onNavigateToSearchResults = { productID ->
                        navController.navigate(
                            route = AppScreens.SingleProductInfo.route.replace(
                                "{product_id}",
                                productID
                            )
                        ){
                            popUpTo(AppScreens.SingleProductInfo.route) {
                                inclusive = true
                            }
                        }
                    },
                    onNavigateToReporte = {
                        navController.navigate(route = AppScreens.Offer.route)
                    },
                    onNavigateToCart = {
                        navController.navigate(route = AppScreens.Cart.route) {
                            popUpTo(route = AppScreens.Cart.route)
                        }
                    },
                )
            }
        }

        /**Other Screens*/
        composable(route = AppScreens.SearchResults.route) { navBackStackEntry ->
            val query = navBackStackEntry.arguments?.getCharSequence("query")
            if (query == null) {
                SearchResultsScreen(
                    onNavigateToSingleProduct = { productID ->
                        navController.navigate(
                            route = AppScreens.SingleProductInfo.route.replace(
                                "{product_id}",
                                productID
                            )
                        )
                    },
                )
            } else {
                SearchResultsScreen(
                    startingQuery = query.toString(),
                    onNavigateToSingleProduct = { productID ->
                        navController.navigate(
                            route = AppScreens.SingleProductInfo.route.replace(
                                "{product_id}",
                                productID
                            )
                        )
                    },
                )
            }
        }

        composable(route = AppScreens.Favorites.route) {
            FavoritesScreen(
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.Explore.route
                    )
                },
                onNavigationLogin = {
                    navController.navigate(
                        route = AppScreens.Login.route
                    )
                },
                onNavigationExplore = { productID ->
                    Constants.corredor = productID
                    navController.navigate(
                        route = AppScreens.Explore.route
                    )
                },
                onNavigateToCliente = {productID ->
                    Constants.corredor = productID
                    navController.navigate(
                        route = AppScreens.Cliente.route
                    )
                }
            )
        }
        composable(route = AppScreens.MapsSucursal.route) {
            MapsSucursalScreen(
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.Explore.route
                    )
                },
                onNavigateToVisitadores = { claveSucursal, fecha ->
                    navController.navigate(
                        route = AppScreens.VisitadorActivo.route.replace(
                            "{claveSucursal}",
                            claveSucursal
                        ).replace(
                            "{fecha}",
                            fecha
                        )
                    )
                }
            )
        }
        composable(route = AppScreens.VisitadorActivo.route,
            arguments = listOf(
                navArgument("claveSucursal") { type = NavType.StringType },
                navArgument("fecha") { type = NavType.StringType }
            )

        ) { navBackStackEntry ->

            val claveSucursal = navBackStackEntry.arguments?.getString("claveSucursal")

            val fecha = navBackStackEntry.arguments?.getString("fecha")

            if (claveSucursal != null && fecha != null) {

                VisitadorActivoScreen(
                    onNavigationBack = {
                        navController.navigate(route = AppScreens.MapsSucursal.route)
                    },

                    onNavigateToRuta = { numeroEmpleado ->

                        navController.navigate(

                            route = AppScreens.RutaVisitador.route.replace(

                                "{claveSucursal}",

                                claveSucursal

                            ).replace(

                                "{numeroEmpleado}",

                                numeroEmpleado

                            ).replace(

                                "{fecha}",

                                fecha

                            )

                        )

                    },

                    claveSucursal = claveSucursal,

                    fecha = fecha

                )

            }

        }



        // Navegación de RutaVisitador

        composable(route = AppScreens.RutaVisitador.route,
            arguments = listOf(

                navArgument("claveSucursal") { type = NavType.StringType },

                navArgument("numeroEmpleado") { type = NavType.StringType },

                navArgument("fecha") { type = NavType.StringType }

            )

        ) { navBackStackEntry ->

            val claveSucursal = navBackStackEntry.arguments?.getString("claveSucursal")

            val numeroEmpleado = navBackStackEntry.arguments?.getString("numeroEmpleado")

            val fecha = navBackStackEntry.arguments?.getString("fecha")



            if (claveSucursal != null && numeroEmpleado != null && fecha != null) {

                RutaVisitadorScreen(

                    onNavigationBack = {

                        navController.navigate(

                            route = AppScreens.VisitadorActivo.route.replace(

                                "{claveSucursal}",

                                claveSucursal

                            ).replace(

                                "{fecha}",

                                fecha

                            )

                        )

                    },

                    claveSucursal = claveSucursal,

                    numeroEmpleado = numeroEmpleado,

                    fecha = fecha

                )

            }

        }

        composable(route = AppScreens.HistorialCotizacion.route) {
            HisCotizacionScreen(
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.Explore.route
                    )
                },
                onNavigateToCart = {
                    navController.navigate(route = AppScreens.Cart.route) {
                        popUpTo(route = AppScreens.Home.route)
                    }
                },
                onNavigateToExplore = {
                    navController.navigate(route = AppScreens.Explore.route) {
                        popUpTo(route = AppScreens.Home.route)
                    }
                },
                onNavigateToTicket = {
                    navController.navigate(route = AppScreens.HistorialCotizacion.route) {
                        popUpTo(route = AppScreens.HistorialCotizacion.route)
                    }
                },
                onNavigateToAccount = {
                    navController.navigate(route = AppScreens.Account.route) {
                        popUpTo(route = AppScreens.Home.route)
                    }
                },
                onNavigateToDetail = { idCotizacion ->
                    navController.navigate(
                        route = AppScreens.HistorialCotizacionDetalle.route.replace(
                            "{idCotizacion}",
                            idCotizacion

                        )

                    )
                },
                )
        }
        composable(route = AppScreens.HistorialCotizacionGerente.route) {
            HisCotizacionGScreen(
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.Explore.route
                    )
                },
                onNavigateToUbicacion = {
                    navController.navigate(route = AppScreens.MapsSucursal.route) {
                        popUpTo(route = AppScreens.MapsSucursal.route)
                    }
                },
                onNavigateToExplore = {
                    navController.navigate(route = AppScreens.Explore.route) {
                        popUpTo(route = AppScreens.Home.route)
                    }
                },
                onNavigateToTicketGerente = {
                    navController.navigate(route = AppScreens.HistorialCotizacionGerente.route) {
                        popUpTo(route = AppScreens.HistorialCotizacionGerente.route)
                    }
                },
                onNavigateToAccount = {
                    navController.navigate(route = AppScreens.Account.route) {
                        popUpTo(route = AppScreens.Home.route)
                    }
                },
                onNavigateToDetail = { idCotizacion, idVistador ->
                    navController.navigate(
                        route = AppScreens.HisCotizacionDetalleGerente.route.replace(
                            "{fecha}",
                            idCotizacion
                        ).replace(
                            "{idVistador}",
                            idVistador
                        )
                    )
                },
            )
        }
        // navegacion para ver historial de cotizaciones detalle visitador

        composable(route = AppScreens.HistorialCotizacionDetalle.route
            ) {
                navBackStackEntry ->
            val idCotizacion = navBackStackEntry.arguments?.getCharSequence("idCotizacion")
            HisCtoDetalleScreen(
               idCotizacion = idCotizacion.toString(),
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.HistorialCotizacion.route
                    )
                },
            )
        }

        composable(route = AppScreens.HisCtoDetalleGerenteId.route
        ) {
                navBackStackEntry ->
            val idCotizacion = navBackStackEntry.arguments?.getCharSequence("idCotizacion")
            HisCtoDetalleGerenteScreen(
                idCotizacion = idCotizacion.toString(),
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.HisCotizacionDetalleGerente.route.replace(
                            "{fecha}",
                            Constants.fechaGDetalle
                        ).replace(
                            "{idVistador}",
                            Constants.idVisitadorGD
                        )
                    )
                },
            )
        }


        composable(route = AppScreens.HisCotizacionDetalleGerente.route
        ) {
                navBackStackEntry ->
            val fecha = navBackStackEntry.arguments?.getCharSequence("fecha")
            val idVistador = navBackStackEntry.arguments?.getCharSequence("idVistador")
            Constants.fechaGDetalle = fecha.toString()
            Constants.idVisitadorGD = idVistador.toString()
            HistCotizacionGDScreen(
                fecha = fecha.toString(),
                idVistador = idVistador.toString(),
                onNavigationBack = {
                    navController.navigate(
                        route = AppScreens.HistorialCotizacionGerente.route
                    )
                },
                onNavigateToDetail = { idCotizacion, fecha, idVisitador ->
                    navController.navigate(
                        route = AppScreens.HisCtoDetalleGerenteId.route.replace(
                            "{idCotizacion}",
                            idCotizacion
                        ).replace(
                            "{fecha}",
                            fecha
                        ).replace(
                            "{idVisitador}",
                            idVisitador
                        )
                    )
                },
            )
        }
    }
}