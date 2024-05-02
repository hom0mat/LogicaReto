LIBRARY ieee;
USE ieee.std_logic_1164.all;
USE ieee.std_logic_unsigned.all;

-- This module can be used to debounce a KEY on the DE10-Lite board
ENTITY debounce IS
	PORT (	Clock			:	IN			STD_LOGIC;
				button		:	IN			STD_LOGIC;
				debounced	:	BUFFER	STD_LOGIC);
END debounce;

ARCHITECTURE Behavior OF debounce IS
	SIGNAL COUNT : STD_LOGIC_VECTOR(2 DOWNTO 0);
	SIGNAL done, counting : STD_LOGIC;
BEGIN	
	PROCESS
	BEGIN
		WAIT UNTIL Clock'EVENT AND Clock = '1';
		IF (done = '1' AND button = '1') THEN	-- When counter expires and button is no longer 
				  debounced <= '0';					-- pressed, then set the debounced output to 0
		ELSIF (button = '0') THEN					-- When the "button" is pressed
			debounced <= '1';							-- set the debounced output to 1
		END IF;
	END PROCESS;

	PROCESS
	BEGIN
		WAIT UNTIL Clock'EVENT AND Clock = '1';
		IF (done = '1') THEN
			Count <= "000";
		ELSIF (debounced = '1') THEN
			Count <= Count + '1';
		END IF;
	END PROCESS;

	done <= '1' WHEN Count = "111" ELSE '0';
END Behavior;