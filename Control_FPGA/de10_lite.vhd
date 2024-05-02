-- Código principal para controlar la FPGA DE10-LITE y conectarla con el procesador simulado
-- Gumnut. L o anterior controla a su vez el movimiento de un tractor en Unity.
-- Hecho por:
-- César Mateo Sánchez Álvarez
-- Harum Amairani Kim Pelayo
-- Brenda Cruz Arango 
-- Natalia Catalina Taboada Maldonado

LIBRARY 	ieee; -- Importe de librerías
USE		ieee.std_logic_1164.all;
USE 		ieee.numeric_std.all;

-- Declaración de Entidad principal del proyecto y su conexión con el hardware de la placa
ENTITY de10_lite IS
	--Valor genérico para definir la anchura de los datos
	GENERIC(
			d_width		: INTEGER 	:= 8);
	PORT(
    CLOCK_50    : IN  std_logic;	--Señal de reloj
	 -- Integración con hardware de la placa
    KEY         : IN  std_logic_vector(1 DOWNTO 0);
	 SW          : IN  std_logic_vector(9 DOWNTO 0);
    LEDR        : OUT std_logic_vector(9 DOWNTO 0);
    HEX0        : OUT std_logic_vector(7 DOWNTO 0);
	 -- Pines para la transmisión y recepción de datos con la UART
	 GPIO_24     : IN  std_logic;    -- RX
    GPIO_25     : OUT std_logic;    -- TX
	 -- Elementos de comunicación con el acelerometro
    GSENSOR_INT : IN  std_logic_vector(1 DOWNTO 0);
    GSENSOR_SDI : INOUT  std_logic;
    GSENSOR_SDO : INOUT  std_logic;
    GSENSOR_CS_N: OUT std_logic;
    GSENSOR_SCLK: OUT std_logic
	);
END;

-- Estructura del comportamiento del proyecto
ARCHITECTURE Structural OF de10_lite IS
-- Declaración de componentes.

-- Componente para habilitar un reset 
COMPONENT reset_delay IS
	PORT( 	iRSTN	: IN std_logic;
		iCLK	: IN std_logic;
		oRST	: OUT	std_logic
	);
END COMPONENT;

-- Componentes para la comunicación con el acelerometro
COMPONENT spi_pll IS
	PORT( 	areset	: IN std_logic;
		inclk0	: IN std_logic;
		c0	: OUT	std_logic;
		c1	: OUT std_logic
	);
END COMPONENT;
COMPONENT spi_ee_config IS
	PORT( 	iRSTN		: IN std_logic;
		iSPI_CLK	: IN std_logic;
		iSPI_CLK_OUT	: IN	std_logic;
		iG_INT2		: IN std_logic;
		oDATA_L		: OUT std_logic_vector(7 DOWNTO 0);
		oDATA_H		: OUT std_logic_vector(7 DOWNTO 0);
		SPI_SDIO	: INOUT std_logic;
		oSPI_CSN	: OUT std_logic;
		oSPI_CLK	: OUT std_logic
	);
END COMPONENT;

-- Componente para habilitar la comunicación serial UART
COMPONENT uart IS
  GENERIC(
    clk_freq  :  integer    := 50_000_000;  --frequency of system clock in Hertz
    baud_rate :  integer    := 115_200;     --data link baud rate in bits/second
    os_rate   :  integer    := 16;          --oversampling rate to find center of receive bits (in samples per baud period)
    d_width   :  integer    := 8;           --data bus width
    parity    :  integer    := 0;           --0 for no parity, 1 for parity
    parity_eo :  std_logic  := '0');        --'0' for even, '1' for odd parity
  PORT(
    clk      :  IN   std_logic;                             --system clock
    reset_n  :  IN   std_logic;                             --ascynchronous reset
    tx_ena   :  IN   std_logic;                             --initiate transmission
    tx_data  :  IN   std_logic_vector(d_width-1 DOWNTO 0);  --data to transmit
    rx       :  IN   std_logic;                             --receive pin
    rx_busy  :  OUT  std_logic;                             --data reception in progress, LEDR(9)
    rx_error :  OUT  std_logic;                             --start, parity, or stop bit error detected
    rx_data  :  OUT  std_logic_vector(d_width-1 DOWNTO 0);  --data received
    tx_busy  :  OUT  std_logic;                             --transmission in progress, LEDR(8)
    tx       :  OUT  std_logic);                            --transmit pin
END COMPONENT;

-- Componente para mejorar el desempeño del botón y eliminar ruido
COMPONENT debounce IS
	PORT (	Clock			:	IN			STD_LOGIC;
				button		:	IN			STD_LOGIC;
				debounced	:	BUFFER	STD_LOGIC);
END COMPONENT;

-- Componente para la visualización del movimiento del acelerometro en los LED's
COMPONENT led_driver IS
	PORT( 	iRSTN	: IN std_logic;
		iCLK	: IN std_logic;
		iDIG	: IN std_logic_vector(9 DOWNTO 0);
		iG_INT2	: IN std_logic;
		oLED	: OUT std_logic_vector(9 DOWNTO 0)
	);
END COMPONENT;

-- Componente para la comunicación con el procesador Gumnut
component gumnut_with_mem IS
		generic ( 
		-- Archivos para el manejo del código ensamblador
			IMem_file_name : string := "gasm_text.dat"; 
			DMem_file_name : string := "gasm_data.dat";
         debug : boolean := false );
		port ( clk_i : in std_logic;
         rst_i : in std_logic; -- Señal de reseteo
         -- I/O port bus
         port_cyc_o : out std_logic;
         port_stb_o : out std_logic;
         port_we_o : out std_logic;
         port_ack_i : in std_logic;
         port_adr_o : out unsigned(7 downto 0);
         port_dat_o : out std_logic_vector(7 downto 0);
         port_dat_i : in std_logic_vector(7 downto 0);
         -- Interrupts
         int_req : in std_logic;
         int_ack : out std_logic );
	end component gumnut_with_mem;

-- Declaración de señales a utilizar
-- Señales para el Gumnut
SIGNAL clk_i, rst_i: std_logic; 
SIGNAL port_cyc_o, port_stb_o, port_we_o, port_ack_i:	std_logic;
SIGNAL port_adr_o:	unsigned(7 downto 0);
SIGNAL port_dat_o, port_dat_i:	std_logic_vector(7 downto 0);
SIGNAL int_req, int_ack: std_logic;

-- Señales para la UART
SIGNAL dly_rst: std_logic;
SIGNAL spi_clk: std_logic;
SIGNAL spi_clk_out: std_logic;
SIGNAL data_x:	std_logic_vector(15 DOWNTO 0);
SIGNAL LEDR_2: std_logic_vector(9 DOWNTO 0);
SIGNAL tx_ena_de10: 		std_logic := '0';
SIGNAL tx_data_de10: 	std_logic_vector(7 DOWNTO 0);
SIGNAL rx_busy_de10: 	std_logic;
SIGNAL rx_error_de10:	std_logic;
SIGNAL rx_data_de10:		std_logic_vector(7 DOWNTO 0);
SIGNAL tx_busy_de10:		std_logic;
SIGNAL key1_db: 			std_logic;
SIGNAL key1_db_past:		std_logic := '0';
SIGNAL inrx_data : STD_LOGIC_VECTOR(d_width-1 DOWNTO 0);
SIGNAL test: std_logic := '0';

BEGIN
	-- Enlace entre señales de gumnut y la entidad general
	clk_i 		<= CLOCK_50;
	rst_i 		<= not KEY( 0 );
	port_ack_i	<= '1';	
	
	-- Instanciación de componentes
					-- Gumnut
	gumnut : 		COMPONENT gumnut_with_mem 
							PORT MAP(
								clk_i,
								rst_i,
								port_cyc_o,
								port_stb_o,
								port_we_o,
								port_ack_i,
								port_adr_o( 7 DOWNTO 0 ),
								port_dat_o( 7 DOWNTO 0 ),
								port_dat_i( 7 DOWNTO 0 ),
								int_req,
								int_ack
								);									
					--UART
	uart_0	: uart 		PORT MAP( CLOCK_50, KEY(0), tx_ena_de10, tx_data_de10, GPIO_24, rx_busy_de10, rx_error_de10, rx_data_de10, tx_busy_de10, GPIO_25);
					--Botón
	button_0	: debounce	PORT MAP( CLOCK_50, KEY(1), key1_db );	
					--Reset
	reset		: reset_delay 	PORT MAP( KEY(0), CLOCK_50, dly_rst );
					--PLL
	pll		: spi_pll 	PORT MAP( dly_rst, CLOCK_50, spi_clk, spi_clk_out );
					--Initial Setting and Data Read Back
	spi_config	: spi_ee_config	PORT MAP( not dly_rst, spi_clk, spi_clk_out, GSENSOR_INT(0), data_x(7 DOWNTO 0), 
							data_x(15 DOWNTO 8), GSENSOR_SDI, GSENSOR_CS_N, GSENSOR_SCLK );
					--LED's
	LEDR <= LEDR_2;
	led		: led_driver 	PORT MAP( not dly_rst, CLOCK_50, data_x(9 DOWNTO 0), GSENSOR_INT(0), LEDR_2 );
	
	--Procesos de control sensibles al reloj de sincronización	
	--Proceso de acelerómetro, botón y switches a través de comunicación serial
	PROCESS(CLOCK_50)
	
	VARIABLE comp : STD_LOGIC_VECTOR(9 DOWNTO 0) := "0000000001";
	
	BEGIN
		 IF rising_edge(CLOCK_50) THEN
			  -- Resetear valores por defecto
			  tx_ena_de10 <= '1';
			  tx_data_de10 <= "00000000";
			
			  -- Si hay un cambio en el estado del botón, se procesa lo siguiente
			  IF key1_db /= key1_db_past THEN
					-- Se habilita la transmisión de datos y se concatenan los valores de
					-- switches para indicar arriba/abajo, bits izquierda/derecha, valor del botón
					-- y switches de velocidad, herramientas de sembrar/cosechar
					tx_ena_de10 <= '0';
					tx_data_de10 <= SW(9 DOWNTO 8) & "00" & key1_db & SW(2 DOWNTO 0);
					-- Si los valores de los LED's indican movimiento hacia la derecha, se indica con 01
					IF LEDR_2 < (comp(6 DOWNTO 0) & "000") THEN
						 tx_data_de10 <= SW(9 DOWNTO 8) & "01" & key1_db & SW(2 DOWNTO 0);
					-- Si los valores de los LED's indican movimiento hacia la izquierda, se indica con 10
					ELSIF LEDR_2 > (comp(2 DOWNTO 0) & "0000000") THEN
						 tx_data_de10 <= SW(9 DOWNTO 8) & "10" & key1_db & SW(2 DOWNTO 0);
					END IF;
				-- Si no hay cambio en el estado del botón, se sigue realizando los demás procesos
				ELSIF key1_db = key1_db_past THEN
					tx_ena_de10 <= '0';
					tx_data_de10 <= SW(9 DOWNTO 8) & "00" & key1_db & SW(2 DOWNTO 0);
					IF LEDR_2 < (comp(6 DOWNTO 0) & "000") THEN
						 tx_data_de10 <= SW(9 DOWNTO 8) & "01" & key1_db & SW(2 DOWNTO 0);
					ELSIF LEDR_2 > (comp(2 DOWNTO 0) & "0000000") THEN
						 tx_data_de10 <= SW(9 DOWNTO 8) & "10" & key1_db & SW(2 DOWNTO 0);
					END IF;
				END IF;
			  -- Guardar el estado actual de key1_db para el próximo ciclo
			  key1_db_past <= key1_db;
		 END IF;
	END PROCESS;
	
	--Proceso del display de 7 segmentos con la recepción de los datos de la comunicación UART
	--Output => Contador en display 7 segmentos HEX0
	PROCESS(CLOCK_50)
	BEGIN
		IF rising_edge( CLOCK_50 ) THEN 
			IF (port_cyc_o = '1' and port_stb_o = '1' and port_we_o = '1' and port_adr_o(1) = '0' and port_adr_o(0) = '1') THEN	
				CASE port_dat_o(3 DOWNTO 0) IS
					WHEN "0000" => HEX0(7 DOWNTO 0) <= "11000000";
					WHEN "0001" => HEX0(7 DOWNTO 0) <= "11111001";
					WHEN "0010" => HEX0(7 DOWNTO 0) <= "10100100";
					WHEN "0011" => HEX0(7 DOWNTO 0) <= "10110000";
					WHEN "0100" => HEX0(7 DOWNTO 0) <= "10011001";
					WHEN "0101" => HEX0(7 DOWNTO 0) <= "10010010";
					WHEN "0110" => HEX0(7 DOWNTO 0) <= "10000010";
					WHEN "0111" => HEX0(7 DOWNTO 0) <= "11111000";
					WHEN "1000" => HEX0(7 DOWNTO 0) <= "10000000";
					WHEN "1001" => HEX0(7 DOWNTO 0) <= "10010000";
					WHEN OTHERS => HEX0(7 DOWNTO 0) <= "11111111";
				END CASE;
			END IF;
		END IF;	
	END PROCESS;
	 
	--Input => Datos de la comunicación UART de las vidas en el juego de UNITY
	PROCESS( CLOCK_50 )
	BEGIN
		IF rising_edge( CLOCK_50 ) THEN
			IF (port_cyc_o = '1' and port_stb_o = '1' and port_we_o = '0' and port_adr_o(1) = '0' and port_adr_o(0) = '0') THEN
				port_dat_i(3 DOWNTO 0) <= rx_data_de10(3 DOWNTO 0);
			END IF;
		END IF;
	END PROCESS;
							
END Structural;
